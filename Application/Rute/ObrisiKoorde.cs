using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Rute
{
    public class ObrisiKoorde
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid RutaId { get; set; }
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

                if (ruta == null) return Result<Unit>.Failure("Nije pronadjena takva ruta!");

                _context.KoordinateRute.RemoveRange(ruta.Koordinate);
                ruta.Koordinate.Clear();

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result) return Result<Unit>.Failure("Failed to delete coordinates");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}