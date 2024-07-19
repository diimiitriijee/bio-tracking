using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Komentari;

public class Create
{
    public class Command : IRequest<Result<KomentarDto>>
    {
        public string Tekst { get; set; }
        public Guid ObilazakId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Tekst).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<KomentarDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IKorisnikAccessor _korisnikAccessor;

        public Handler(DataContext context, IMapper mapper, IKorisnikAccessor korisnikAccessor)
        {
            _korisnikAccessor = korisnikAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<KomentarDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var obilazak = await _context.Obilasci
                .Include(x => x.Komentari)
                .ThenInclude(x => x.Korisnik)
                .ThenInclude(x => x.Slike)
                .FirstOrDefaultAsync(x => x.ID == request.ObilazakId);

            if (obilazak == null) return null;

            var korisnik = await _context.Users
                .SingleOrDefaultAsync(x => x.UserName == _korisnikAccessor.GetUsername());

            var komentar = new Komentar
            {
                Korisnik = korisnik,
                Obilazak = obilazak,
                Tekst = request.Tekst
            };

            obilazak.Komentari.Add(komentar);

            var success = await _context.SaveChangesAsync() > 0;

            if (success) return Result<KomentarDto>.Success(_mapper.Map<KomentarDto>(komentar));

            return Result<KomentarDto>.Failure("Failed to add comment");
        }
    }
}