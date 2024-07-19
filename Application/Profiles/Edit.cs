using Application.Core;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumRodjenja { get; set; }
        //public string SlikaProfila { get; set; }
        //public string UserName { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Ime).NotEmpty();
            RuleFor(x => x.Prezime).NotEmpty();
            RuleFor(x => x.Telefon).NotEmpty();
            RuleFor(x => x.DatumRodjenja).NotEmpty();
            //RuleFor(x => x.UserName).NotEmpty();
        }
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
            var korisnik = await _context.Korisnici.FirstOrDefaultAsync(x =>
                x.UserName == _korisnikAccessor.GetUsername());

            korisnik.Ime = request.Ime ?? korisnik.Ime;
            korisnik.Prezime = request.Prezime ?? korisnik.Prezime;
            korisnik.Telefon = request.Telefon ?? korisnik.Telefon;
            //korisnik.UserName = request.UserName ?? korisnik.UserName;
            korisnik.DatumRodjenja = request.DatumRodjenja;// da li je dobro?
            //korisnik.SlikaProfila = request.SlikaProfila ?? korisnik.SlikaProfila;

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<Unit>.Success(Unit.Value);
            
            return Result<Unit>.Failure("Problem updating profile");
        }
    }
}