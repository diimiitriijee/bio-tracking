using Domain;
using FluentValidation;

namespace Application.Korisnici;

public class KorisnikValidator : AbstractValidator<Korisnik>
{
    public KorisnikValidator()
    {
        // RuleFor(x => x.Ime).NotEmpty();
        // RuleFor(x => x.Prezime).NotEmpty();
        // RuleFor(x => x.Telefon).NotEmpty();
        // RuleFor(x => x.DatumRodjenja).NotEmpty();
        // RuleFor(x => x.Email).NotEmpty();
        // RuleFor(x => x.UserName).NotEmpty();
        // RuleFor(x => x.SlikaProfila).NotEmpty();
    }
}
