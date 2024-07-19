using Application.Core;
using MediatR;
using Persistence;

namespace Application.Vodici;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
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
            var vodic = await _context.Vodici.FindAsync(request.Id);
            if (vodic == null) return null;
            _context.Remove(vodic);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the vodic");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
