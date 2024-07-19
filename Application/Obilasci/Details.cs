using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;

public class Details
{
    public class Query : IRequest<Result<ObilazakDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ObilazakDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<ObilazakDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                                    .Include(o => o.Ruta).ThenInclude(k => k.Koordinate.OrderBy(k => k.CreatedAt))
                                    .ProjectTo<ObilazakDto>(_mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync(o => o.ID == request.Id);
            
            if (obilazak == null)
            {
                return Result<ObilazakDto>.Failure("Obilazak nije pronađen.");
            }
            return Result<ObilazakDto>.Success(obilazak);
        }
    }
}
