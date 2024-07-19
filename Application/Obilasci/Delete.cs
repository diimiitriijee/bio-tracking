using Application.Core;
using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Obilasci;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailSender;
        public Handler(DataContext context, IEmailService emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci.Include(o => o.Vodic).Include(u => u.Ucesnici).ThenInclude(k => k.Korisnik).FirstOrDefaultAsync(x => x.ID == request.Id);
            if (obilazak == null) return Result<Unit>.Failure("Nije pronadjen obilazak");
            var message = $"<p>Poštovani korisniče, obaveštavamo Vas da je vodič: {obilazak.Vodic.Ime} {obilazak.Vodic.Prezime} otkazao obilazak: {obilazak.Naziv}. Hvala na razumevanju, Vaš BioTS.</p>";
            var ucesnici = obilazak.Ucesnici;
            var vodic = await _context.Vodici.FindAsync(obilazak.Vodic?.Id);
            if (vodic != null && vodic.BrojOdrzanihObilazaka > 0)
            {
                vodic.BrojOdrzanihObilazaka--;
            }
            _context.Remove(obilazak);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the obilazak");
            
            if(ucesnici != null)
            {
                foreach(var ucesnik in ucesnici)
                {
                    await _emailSender.SendGridEmailAsync(ucesnik.Korisnik.Email, "Otkazan obilazak", message);
                }
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
