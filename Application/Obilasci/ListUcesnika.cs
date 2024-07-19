using Application.Core;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;

public class ListUcesnika
{
    public class Query : IRequest<Result<List<Profiles.Profile>>>
    {
        public Guid IdObilaska { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                .Include(o => o.Ucesnici).ThenInclude(u => u.Korisnik)
                .FirstOrDefaultAsync(o => o.ID == request.IdObilaska);

            //var ucesnici = await _context.Users.Where(o => o.PrijavljeniObilasci == obilazak).ToListAsync();

            if (obilazak == null) return Result<List<Profiles.Profile>>.Failure("Obilazak nije pronađen");

            var ucesnici = _mapper.Map<List<Profiles.Profile>>(obilazak.Ucesnici);
            return Result<List<Profiles.Profile>>.Success(ucesnici);
        }
    }
}