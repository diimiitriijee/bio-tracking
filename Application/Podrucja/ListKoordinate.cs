using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class ListKoordinate
{
    public class Query : IRequest<Result<List<KoordinatePodrucja>>>
    {
        public Guid PodrucjeId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<KoordinatePodrucja>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<KoordinatePodrucja>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var podrucje = await _context.Podrucja
                .Include(p => p.Koordinate)
                .FirstOrDefaultAsync(p => p.ID == request.PodrucjeId);

            if (podrucje == null) return null;

            return Result<List<KoordinatePodrucja>>.Success(podrucje.Koordinate.ToList());
        }
    }
}