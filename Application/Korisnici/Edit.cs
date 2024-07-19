using Application.Core;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Korisnici;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public Korisnik Korisnik { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Korisnik).SetValidator(new KorisnikValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var korisnik = await _context.Korisnici.FindAsync(request.Korisnik.Id);
             if (korisnik == null) return null;
            _mapper.Map(request.Korisnik, korisnik);
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to update korisnik");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
