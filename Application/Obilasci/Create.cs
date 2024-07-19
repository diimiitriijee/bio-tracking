using Application.Core;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;
public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Obilazak Obilazak { get; set; }
        public Guid IdPodrucja { get; set; }
        public Guid IdRute {get; set; }
    }
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Obilazak).SetValidator(new ObilazakValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IKorisnikAccessor _korisnikAccessor;
        private readonly IEmailService _emailSender;
        public Handler(DataContext context, IKorisnikAccessor korisnikAccessor, IEmailService emailSender)
        {
            _emailSender = emailSender;
            _korisnikAccessor = korisnikAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var korisnik = _context.Vodici.Include(o => o.Followers).ThenInclude(k => k.Observer).FirstOrDefault(x => 
                    x.UserName == _korisnikAccessor.GetUsername());

            if(korisnik ==null) return Result<Unit>.Failure("Nije pronadjen vodic!");
            request.Obilazak.Vodic = korisnik;
            request.Obilazak.Vodic.BrojOdrzanihObilazaka++;

            var podrucje = await _context.Podrucja.FindAsync(request.IdPodrucja);
            if(podrucje == null) return Result<Unit>.Failure("Nije pronadjeno podrucje!");

            var ruta = await _context.Rute.FindAsync(request.IdRute);
            if(ruta == null) return Result<Unit>.Failure("Nije pronadjena ruta!");

            request.Obilazak.Podrucje = podrucje;//pazi ovde mozda treba podrucje.Obilasci.Add
            request.Obilazak.Ruta = ruta;//a i ovde verovatno ruta.Obilasci.Add a to jos nisi ni dodao u migraciju

            _context.Obilasci.Add(request.Obilazak);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to create obilazak");

            var message = $"<p>Poštovani korisniče, obaveštavamo Vas da je vodič: {korisnik.Ime} {korisnik.Prezime} objavio novi obilazak: {request.Obilazak.Naziv}. Prijave su u toku, Vaš BioTS.</p>";
            var pratioci = korisnik.Followers;

            foreach(var pratioc in pratioci)
            {
                await _emailSender.SendGridEmailAsync(pratioc.Observer.Email, "Novi obilazak", message);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
