using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Korisnici;

public class Details
{
    public class Query : IRequest<Result<Korisnik>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Korisnik>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Korisnik>> Handle(Query request, CancellationToken cancellationToken)
        {
            var korisnik = await _context.Korisnici
                .Include(k => k.PrijavljeniObilasci)
                .FirstOrDefaultAsync(k => k.Id == request.Id, cancellationToken);

            if (korisnik == null)
            {
                return Result<Korisnik>.Failure("Korisnik nije pronađen");
            }

            return Result<Korisnik>.Success(korisnik);
        }
    }
}
