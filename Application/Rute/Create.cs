using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Rute;
public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public Ruta Ruta { get; set; }
        public Guid PodrucjeId {get; set;}
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Ruta).SetValidator(new RutaValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var podrucje = await _context.Podrucja.FindAsync(request.PodrucjeId);
            if(podrucje == null) return Result<Unit>.Failure("Nije pronadjeno podrucje!");

            request.Ruta.Podrucje = podrucje;
            podrucje.Rute.Add(request.Ruta);

            _context.Rute.Add(request.Ruta);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Neuspelo kreiranje rute");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
