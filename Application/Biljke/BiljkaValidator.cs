using Domain;
using FluentValidation;

namespace Application.Biljke;

public class BiljkaValidator : AbstractValidator<Biljka>
{
    public BiljkaValidator()
    {
        RuleFor(x => x.Naziv).NotEmpty();
        RuleFor(x => x.Opis).NotEmpty();
        RuleFor(x => x.Vrsta).NotEmpty();
        //RuleFor(x => x.Lekovita).NotNull();
    }
}
