using Domain;
using FluentValidation;

namespace Application.Rute;

public class RutaValidator : AbstractValidator<Ruta>
{
    public RutaValidator()
    {
        RuleFor(x => x.Prohodnost).NotEmpty();
        RuleFor(x => x.Duzina).NotEmpty();
        RuleFor(x => x.Opis).NotEmpty();
    }
}