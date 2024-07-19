using Domain;
using FluentValidation;

namespace Application.Obilasci;

public class ObilazakValidator : AbstractValidator<Obilazak>
{
    public ObilazakValidator()
    {
        RuleFor(x => x.Naziv).NotEmpty();
        RuleFor(x => x.Opis).NotEmpty();
        RuleFor(x => x.DatumOdrzavanja).NotEmpty();
        RuleFor(x => x.BrojMaxPolaznika).NotEmpty();
        RuleFor(x => x.MestoOkupljanja).NotEmpty();
    }
}