using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;


public class IzmeniKoorde
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid PodrucjeId { get; set; }
        public Koordinata[] Koordinate {get; set;}
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
            
            //var podrucje = await _context.Podrucja.FindAsync(request.PodrucjeId);

            if (podrucje == null) return Result<Unit>.Failure("Podrucje not found");
            //brisemo prvo trenutne
            _context.KoordinatePodrucja.RemoveRange(podrucje.Koordinate);
            podrucje.Koordinate.Clear();

            //Postavljamo nove
            foreach(var k in request.Koordinate)
            {
                var koordePorducja = new KoordinatePodrucja
                {
                    Podrucje = podrucje,
                    Latitude = k.Latitude,
                    Longitude = k.Longitude
                };
                podrucje.Koordinate.Add(koordePorducja);
                
            }
            var result = await _context.SaveChangesAsync() > 0;
            if (!result) return Result<Unit>.Failure("Failed to add coordinate");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}