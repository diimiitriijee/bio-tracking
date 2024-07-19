using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Rute;

public class DodajKoordinate
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid RutaId { get; set; }
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
            var ruta = await _context.Rute.FindAsync(request.RutaId);

            if (ruta == null) return null;

            var koordinate = new KoordinateRute
            {
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Ruta = ruta
            };

            ruta.Koordinate.Add(koordinate);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to add coordinate");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}