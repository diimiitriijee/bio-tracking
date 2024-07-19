using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Rute
{
    public class KoordinataRute
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public KoordinataRute()
        {

        }
        public KoordinataRute(double longitude, double latitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
    public class DodajKoorde
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid RutaId { get; set; }
            public KoordinataRute[] Koordinate {get; set;}
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

                if (ruta == null) return Result<Unit>.Failure("Nije pronadjena takva ruta!");;

                foreach(var k in request.Koordinate)
                {
                    var koordeRute = new KoordinateRute
                    {
                        Ruta = ruta,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude
                    };
                    ruta.Koordinate.Add(koordeRute);
                    
                }
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to add coordinate");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}