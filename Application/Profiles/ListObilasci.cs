using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class ListObilasci
{
    public class Query : IRequest<Result<List<KorisnikObilazakDto>>>
    {
        public string Username { get; set; }
        public string Predicate { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<KorisnikObilazakDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<List<KorisnikObilazakDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.PrijavljeniObilasci
                .Where(u => u.Korisnik.UserName == request.Username || u.Obilazak.Vodic.UserName == request.Username)
                .OrderBy(a => a.Obilazak.DatumOdrzavanja)
                .ProjectTo<KorisnikObilazakDto>(_mapper.ConfigurationProvider)
                .AsQueryable();//kao queryable jer tu jos necu da zovem pristup bazi
            //znaci mora jos jedan query koji pretrazuje tabelu obilasci i vraca svaki obilazak koji ima request.Username za vodica i onda dole u hosting da se ispita samo taj drugi query
            //a da bi to radilo mora da imamo mapiranje mrtvo iz obilazak u korisnikObilazakDTO a trenutno je 4:47 tako da odo u kurac jutre ovo
            
            var query2 = _context.Obilasci.OrderBy(o => o.DatumOdrzavanja)
                .ProjectTo<KorisnikObilazakDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var today = DateTime.UtcNow;

            query = request.Predicate switch
            {
                "past" => query.Where(obilazak => obilazak.DatumOdrzavanja < today),// mozda treba da se doda za vodica
                "hosting" => query2.Where(obilazak => obilazak.VodicUsername == request.Username),
                _ => query.Where(obilazak => obilazak.DatumOdrzavanja >= today)
            };

            var obilasci = await query.ToListAsync();//nego tek ovde

            return Result<List<KorisnikObilazakDto>>.Success(obilasci);
        }
    }
}