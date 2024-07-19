using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Biljke;
public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public BiljkaDto Biljka { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            //RuleFor(x => x.Biljka).SetValidator(new BiljkaValidator());
            RuleFor(x => x.Biljka.Naziv).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;
        public Handler(DataContext context, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _photoAccessor = photoAccessor;
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            //var biljka = _mapper.Map<Biljka>(request.Biljka);
            var biljka = new Biljka
            {
                Naziv = request.Biljka.Naziv,
                Opis = request.Biljka.Opis,
                Vrsta = request.Biljka.Vrsta,
                Lekovita = request.Biljka.Lekovita,
                Slika = null
            };

            if (request.Biljka.Slika != null)
            {
                var photoUploadResult = await _photoAccessor.AddPhoto(request.Biljka.Slika);
                biljka.Slika = photoUploadResult.Url;
            }
            _context.Biljke.Add(biljka);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to create biljka");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
