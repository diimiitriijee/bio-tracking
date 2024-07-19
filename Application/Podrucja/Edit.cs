using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Podrucja;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Podrucje Podrucje { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Podrucje).SetValidator(new PodrucjeValidator());
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
            var podrucje = await _context.Podrucja.FindAsync(request.Podrucje.ID);
            if (podrucje == null) return null;
            _mapper.Map(request.Podrucje, podrucje);
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update podrucje");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
