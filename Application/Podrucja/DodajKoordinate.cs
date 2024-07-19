using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Podrucja;

public class DodajKoordinate
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid PodrucjeId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
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

            if (podrucje == null) return null;

            var koordinate = new KoordinatePodrucja
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Podrucje = podrucje
            };

            podrucje.Koordinate.Add(koordinate);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to add coordinate");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}