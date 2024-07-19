using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Komentari;

public class List
{
    public class Query : IRequest<Result<List<KomentarDto>>>
        {
            public Guid ObilazakId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<KomentarDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;
            public Handler(DataContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<Result<List<KomentarDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var comments = await _context.Komentari
                    .Where(x => x.Obilazak.ID == request.ObilazakId)
                    .OrderByDescending(x => x.DatumKreiranja)
                    .ProjectTo<KomentarDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<KomentarDto>>.Success(comments);
            }
        }
}