using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Rute;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Ruta Ruta { get; set; }
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
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var ruta = await _context.Rute.FindAsync(request.Ruta.ID);
            if (ruta == null) return null;
            _mapper.Map(request.Ruta, ruta);
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update ruta");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
