using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Administrator
{
    public class BanujKorisnika
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid KorisnikId {get; set;}
        }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly UserManager<Korisnik> _userManager;    

        public Handler(DataContext context, UserManager<Korisnik> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var korisnik = await _userManager.Users.SingleOrDefaultAsync(k => k.Id == request.KorisnikId);
            if(korisnik == null) return Result<Unit>.Failure("Nije pronadjen korisnik");

            korisnik.LockoutEnabled = true;//ovo bi ovako bilo perma ban jer ne podesim datum isteka, a mozda moze da se napravi ban za manje razmisli
            korisnik.LockoutEnd = DateTime.UtcNow.AddDays(7);//ovako na 7 dana ipak uvek

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Neuspelo banovanje");

            return Result<Unit>.Success(Unit.Value);
        }
    }
    }
}