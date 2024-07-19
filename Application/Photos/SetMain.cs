using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class SetMain
{
    public class Command : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
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
            var korisnik = await _context.Users
                .Include(p => p.Slike)
                .FirstOrDefaultAsync(x => x.UserName == _korisnikAccessor.GetUsername());

            var photo = korisnik.Slike.FirstOrDefault(x => x.Id == request.Id);

            if (photo == null) return null;

            var currentMain = korisnik.Slike.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;
            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Problem setting main photo");
        }
    }
}