using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;

public class List
{
    public class Query : IRequest<Result<PagedList<ObilazakDto>>>
    {
        public ObilazakParams Params  { get; set; }
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
            var query = _context.Obilasci   
                                    .Where(obilazak => obilazak.DatumOdrzavanja >= request.Params.StartDate)//da bi mogo korisnik da izabere od kog datuma da mu se prikazuju obilasci
                                    .OrderBy(obilazak => obilazak.DatumOdrzavanja)//ako neces ovo diko iskljuci
                                    .ProjectTo<ObilazakDto>(_mapper.ConfigurationProvider)
                                    //.ToListAsync(cancellationToken);
                                    .AsQueryable();//kreiramo ga kao query da bismo na osnovu toga napravili nas tip paginirane liste za prikaz sa parametrima koje bira korisnik

            if(request.Params.IsGoing && !request.Params.IsHost)
            {
                query = query.Where(obilazak => obilazak.Ucesnici.Any(ucesnik => ucesnik.Username == _korisnikAccessor.GetUsername()));//samo za ulogovanog
            }

            if(request.Params.IsHost && !request.Params.IsGoing)
            {
                query = query.Where(obilazak => obilazak.Vodic.Username == _korisnikAccessor.GetUsername());//za vodica 
            }

            // Proveravamo da li parametri za paginaciju imaju validne vrednosti
            var pageNumber = request.Params.PageNumber < 1 ? 1 : request.Params.PageNumber;
            var pageSize = request.Params.PageSize < 1 ? 10 : request.Params.PageSize; // Default page size is 10

            var pagedList = await PagedList<ObilazakDto>.CreateAsync(query, pageNumber, pageSize);

            if (pagedList.Count == 0)
            {
                return Result<PagedList<ObilazakDto>>.Success(new PagedList<ObilazakDto>(new List<ObilazakDto>(), 0, pageNumber, pageSize));
            }

            return Result<PagedList<ObilazakDto>>.Success(pagedList);
        }
    }
}