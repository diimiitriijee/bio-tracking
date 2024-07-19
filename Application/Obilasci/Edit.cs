using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Obilasci;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Obilazak Obilazak { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Obilazak).SetValidator(new ObilazakValidator());
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
            var obilazak = await _context.Obilasci.FindAsync(request.Obilazak.ID);
            if (obilazak == null) return null;
            _mapper.Map(request.Obilazak, obilazak);
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update obilazak");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
