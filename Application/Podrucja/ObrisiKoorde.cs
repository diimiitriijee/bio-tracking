using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja
{
    public class ObrisiKoorde
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid PodrucjeId { get; set; }
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
                var podrucje = await _context.Podrucja
                .Include(p => p.Koordinate)
                .FirstOrDefaultAsync(p => p.ID == request.PodrucjeId);

                if (podrucje == null) return Result<Unit>.Failure("Podrucje not found");

                _context.KoordinatePodrucja.RemoveRange(podrucje.Koordinate);
                podrucje.Koordinate.Clear();

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete coordinates");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
