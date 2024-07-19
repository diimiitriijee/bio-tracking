using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Komentari;

public class DeleteAll
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid ObilazakId { get; set; }
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
            var obilazak = await _context.Obilasci
                .Include(x => x.Komentari)
                .FirstOrDefaultAsync(x => x.ID == request.ObilazakId);

            if (obilazak == null) return Result<Unit>.Failure("Tour not found");

            _context.Komentari.RemoveRange(obilazak.Komentari);

            var success = await _context.SaveChangesAsync() > 0;

            if (!success) return Result<Unit>.Failure("Failed to delete comments");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
