using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Vodici;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        //public Vodic Vodic { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Telefon { get; set; }
        public DateTime DatumRodjenja { get; set; }
        //public string SlikaProfila { get; set; }
        //public string UserName { get; set; }
        public string StrucnaSprema { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Ime).NotEmpty();
            RuleFor(x => x.Prezime).NotEmpty();
            RuleFor(x => x.Telefon).NotEmpty();
            RuleFor(x => x.DatumRodjenja).NotEmpty();
            //RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.StrucnaSprema).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikAccessor _korisnikAccessor;
        public Handler(DataContext context, IMapper mapper, IKorisnikAccessor korisnikAccessor)
        {
            _korisnikAccessor = korisnikAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            //var vodic = await _context.Vodici.FindAsync(request.Vodic.Id);
            var vodic = (Vodic)await _context.Users.FirstOrDefaultAsync(x =>
                x.UserName == _korisnikAccessor.GetUsername());
            if (vodic == null) return Result<Unit>.Failure("Nije pronadjen vodic!");
            //_mapper.Map(request.Vodic, (Vodic)vodic);

            vodic.Ime = request.Ime ?? vodic.Ime;
            vodic.Prezime = request.Prezime ?? vodic.Prezime;
            vodic.Telefon = request.Telefon ?? vodic.Telefon;
            //vodic.UserName = request.UserName ?? vodic.UserName;
            vodic.DatumRodjenja = request.DatumRodjenja;// da li je dobro?
            //vodic.Slike = request.SlikaProfila ?? vodic.SlikaProfila;
            vodic.StrucnaSprema = request.StrucnaSprema ?? vodic.StrucnaSprema;
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update vodic!");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
