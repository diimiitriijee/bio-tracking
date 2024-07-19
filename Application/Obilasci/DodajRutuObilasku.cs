using Application.Core;
using MediatR;
using Persistence;

namespace Application.Obilasci
{
    public class DodajRutuObilasku
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid ObilazakId { get; set; }
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
                var obilazak = await _context.Obilasci.FindAsync(request.ObilazakId);

                if (obilazak == null) return Result<Unit>.Failure("Podrucje nije pronadjeno");

                var ruta = await _context.Rute.FindAsync(request.RutaId);

                if (ruta == null) return Result<Unit>.Failure("Biljka nije pronadjena");

                obilazak.Ruta = ruta;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result) return Result<Unit>.Failure("Nije uspelo dodavanje rute obilasku");

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}