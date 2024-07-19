using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class IzbaciBiljku
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
                var podrucje = await _context.Podrucja
                    .Include(p => p.Biljke)
                    .ThenInclude(pb => pb.Biljka)
                    .FirstOrDefaultAsync(p => p.ID == request.PodrucjeId);

                if (podrucje == null) 
                    return Result<Unit>.Failure("Područje nije pronađeno");

                var biljka = podrucje.Biljke
                    .FirstOrDefault(pb => pb.BiljkaID == request.BiljkaId);

                if (biljka == null) 
                    return Result<Unit>.Failure("Biljka nije pronađena na području");

                podrucje.Biljke.Remove(biljka);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) 
                    return Result<Unit>.Failure("Neuspelo izbacivanje biljke");

                return Result<Unit>.Success(Unit.Value);
            }
        }
}