using System.Text.Json.Nodes;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;
public class OcenaModel
{
    public int VrednostOcene { get; set; }
    public string Komentar { get; set; }
}
public class DodajOcenuVodicu
{
    
    public class Command : IRequest<Result<Unit>>
    {
        public Guid IdObilaska { get; set; }
        public int VrednostOcene { get; set; }
        public string Komentar { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IKorisnikAccessor _korisnikAccessor;

        public Handler(DataContext context, IKorisnikAccessor korisnikAccessor)
        {
            _korisnikAccessor = korisnikAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                .Include(o => o.Vodic).ThenInclude(v => v.Ocene)
                .FirstOrDefaultAsync(o => o.ID == request.IdObilaska);

            if (request.VrednostOcene == 0 || request.Komentar == null) return Result<Unit>.Failure("Podaci su neispravni");

            if (obilazak == null) return Result<Unit>.Failure("Obilazak nije pronađen");

            if (obilazak.Vodic == null) return Result<Unit>.Failure("Vodic nije pronađen");

            var korisnik = await _context.Users.Include(k => k.PrijavljeniObilasci).FirstOrDefaultAsync(x => x.UserName == _korisnikAccessor.GetUsername());
    
            if (korisnik == null) return Result<Unit>.Failure("Ucesnik nije pronađen");

            var prijavljeniObilazak = korisnik.PrijavljeniObilasci.Any(po => po.ObilazakID == request.IdObilaska);
            if (prijavljeniObilazak == false) 
                return Result<Unit>.Failure("Niste prijavljeni na ovaj obilazak!");

            
            var postojecaOcena = await _context.Ocene.FirstOrDefaultAsync(o => o.Korisnik == korisnik && o.Vodic == obilazak.Vodic);
            if(postojecaOcena != null) return Result<Unit>.Failure("Vec ste ocenili ovog vodica!");

            var ocena = new Ocena
            {
                Korisnik = korisnik,
                Vodic = obilazak.Vodic,
                VrednostOcene = request.VrednostOcene,
                Komentar = request.Komentar
            };

            if (ocena == null) return Result<Unit>.Failure("Ocena nije napravljena");

            //_context.Ocene.Add(ocena);
            
            if (obilazak.Vodic.Ocene == null) return Result<Unit>.Failure("Ocene vodica su null");

            obilazak.Vodic.Ocene.Add(ocena);
            // float sum = 0;
            // float average = 0;
            // foreach(var o in obilazak.Vodic.Ocene){
            //     sum += o.VrednostOcene;
            // }
            // average = sum/obilazak.Vodic.Ocene.Count;
            
            //if(ocena.VrednostOcene > 0) return Result<Unit>.Failure($"Vodic {obilazak.Vodic.Ime} {obilazak.Vodic.Prezime} ima {obilazak.Vodic.Ocene.Count} ocena, i poslednja je {obilazak.Vodic.Ocene.Last().VrednostOcene} a prosecna mu dodje {average} a ucesnik koji ga je poslednji ocenjivao je {obilazak.Vodic.Ocene.Last().Korisnik.UserName}");

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Nije uspelo ocenjivanje");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}