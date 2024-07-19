using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Rute;

public class ListKoordinate
{
    public class Query : IRequest<Result<List<KoordinateRute>>>
    {
        public Guid RutaId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<KoordinateRute>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<KoordinateRute>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ruta = await _context.Rute
                .Include(p => p.Koordinate)
                .FirstOrDefaultAsync(p => p.ID == request.RutaId);

            if (ruta == null) return null;

            return Result<List<KoordinateRute>>.Success(ruta.Koordinate.ToList());
        }
    }
}