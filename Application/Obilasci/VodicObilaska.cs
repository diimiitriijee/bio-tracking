using Application.Core;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;
public class VodicObilaska
{
    public class Query : IRequest<Result<ProfileVodic>>
    {
        public Guid IdObilaska { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ProfileVodic>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<ProfileVodic>> Handle(Query request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                //.Include(o => o.Vodic).ThenInclude(v => v.Ocene).ThenInclude(v => v.Korisnik)
                .ProjectTo<ObilazakDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(o => o.ID == request.IdObilaska);

            if (obilazak == null) return Result<ProfileVodic>.Failure("Obilazak nije pronađen");

            return Result<ProfileVodic>.Success(obilazak.Vodic);
        }
    }
}