using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Biljke;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Biljka Biljka { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Biljka).SetValidator(new BiljkaValidator());
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
            var biljka = await _context.Biljke.FindAsync(request.Biljka.ID);
            if (biljka == null) return Result<Unit>.Failure("Failed to find biljka");

            _mapper.Map(request.Biljka, biljka);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update biljka");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
