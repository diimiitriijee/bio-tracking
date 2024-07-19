using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Rute
{
    public class IzmeniKoorde
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
                var ruta = await _context.Rute
                    .Include(p => p.Koordinate)
                    .FirstOrDefaultAsync(p => p.ID == request.RutaId);
                

                if (ruta == null) return Result<Unit>.Failure("Nije pronadjena takva ruta");
                //brisemo prvo trenutne
                _context.KoordinateRute.RemoveRange(ruta.Koordinate);
                ruta.Koordinate.Clear();

                //Postavljamo nove
                foreach(var k in request.Koordinate)
                {
                    var koordePorducja = new KoordinateRute
                    {
                        Ruta = ruta,
                        Latitude = k.Latitude,
                        Longitude = k.Longitude
                    };
                    ruta.Koordinate.Add(koordePorducja);
                    
                }
                var result = await _context.SaveChangesAsync() > 0;
                if (!result) return Result<Unit>.Failure("Failed to add coordinate");
                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}