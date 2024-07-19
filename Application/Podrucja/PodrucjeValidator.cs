using Domain;
using FluentValidation;

namespace Application.Podrucja;

public class PodrucjeValidator : AbstractValidator<Podrucje>
{
    public PodrucjeValidator()
    {
        RuleFor(x => x.Oblast).NotEmpty();
    }
}