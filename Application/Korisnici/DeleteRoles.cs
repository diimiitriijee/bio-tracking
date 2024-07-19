using Application.Core;
using MediatR;
using Persistence;

namespace Application.Korisnici;

public class DeleteRoles
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
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
            var role = _context.Roles.FirstOrDefault(x => x.Name == "ObicanKorisnik");//znam
            
            var korisnikRole = await _context.UserRoles.FindAsync(request.Id, role.Id);
            if (korisnikRole == null) Result<Unit>.Failure("Oce kurac tako");
            
            _context.Remove(korisnikRole);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the korisnik");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
