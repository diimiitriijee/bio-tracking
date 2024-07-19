using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Application.Interfaces;
using Domain;

namespace Application.Obilasci;
public class UkloniUcesnika
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid IdObilaska { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IKorisnikAccessor _korisnikAccessor;

        public Handler(DataContext context, IKorisnikAccessor korisnikAccessor)
        {
            _korisnikAccessor = korisnikAccessor;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                .Include(o => o.Ucesnici).ThenInclude(u => u.Korisnik)
                .FirstOrDefaultAsync(o => o.ID == request.IdObilaska);

            if (obilazak == null) return Result<Unit>.Failure("Obilazak nije pronađen");

            var korisnik = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _korisnikAccessor.GetUsername());

            if (korisnik == null) return Result<Unit>.Failure("Ucesnik nije pronađen");

            var ucesnik = obilazak.Ucesnici.FirstOrDefault(x => x.Korisnik.UserName == korisnik.UserName);

            if (ucesnik != null)
            {
                obilazak.Ucesnici.Remove(ucesnik);
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Nije uspelo dodavanje učesnika");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}