using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;

public class ObilasciNaPodrucju
{
    public class Query : IRequest<Result<PagedList<ObilazakDto>>>
    {
        public Guid IdPodrucja { get; set; }
        public ObilazakParams Params { get; set; } // Koristi ObilazakParams umesto PagingParams
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<ObilazakDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikAccessor _korisnikAccessor;

        public Handler(DataContext context, IMapper mapper, IKorisnikAccessor korisnikAccessor)
        {
            _mapper = mapper;
            _context = context;
            _korisnikAccessor = korisnikAccessor;
        }

        public async Task<Result<PagedList<ObilazakDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var podrucje = await _context.Podrucja.FindAsync(request.IdPodrucja);
            if (podrucje == null) return Result<PagedList<ObilazakDto>>.Failure("Podrucje nije pronadjeno");

            var query = _context.Obilasci
                .Where(o => o.Podrucje.ID == podrucje.ID && o.DatumOdrzavanja >= request.Params.StartDate)
                .OrderBy(o => o.DatumOdrzavanja)
                .ProjectTo<ObilazakDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            if (request.Params.IsGoing && !request.Params.IsHost)
            {
                query = query.Where(o => o.Ucesnici.Any(u => u.Username == _korisnikAccessor.GetUsername()));
            }

            if (request.Params.IsHost && !request.Params.IsGoing)
            {
                query = query.Where(o => o.Vodic.Username == _korisnikAccessor.GetUsername());
            }

            var pagedObilasci = await PagedList<ObilazakDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);

            return Result<PagedList<ObilazakDto>>.Success(pagedObilasci);
        }
    }
}
