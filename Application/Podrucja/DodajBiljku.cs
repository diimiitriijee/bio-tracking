using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Podrucja;

public class DodajBiljku
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid PodrucjeId { get; set; }
        public Guid BiljkaId { get; set; }
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

            if (podrucje == null) return Result<Unit>.Failure("Podrucje not found");

            var biljka = await _context.Biljke.FindAsync(request.BiljkaId);

            if (biljka == null) return Result<Unit>.Failure("Biljka not found");

            var povezivanje = new Podrucja_Biljke
            {
                PodrucjeID = podrucje.ID,
                BiljkaID = biljka.ID,
                Podrucje = podrucje,
                Biljka = biljka
            };

            //_context.Podrucja_Biljke.Add(povezivanje);
            podrucje.Biljke.Add(povezivanje);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to add coordinate");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}