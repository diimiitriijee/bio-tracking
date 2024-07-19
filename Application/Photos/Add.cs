using Application.Core;
using Application.Interfaces;
using Domain.src;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos;

public class Add
{
    public class Command : IRequest<Result<Photo>>
    {
        public IFormFile File { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Photo>>
    {
        private readonly DataContext _context;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IKorisnikAccessor _korisnikAccessor;

        public Handler(DataContext context, IPhotoAccessor photoAccessor, IKorisnikAccessor korisnikAccessor)
        {
            _korisnikAccessor = korisnikAccessor;
            _context = context;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
        {
            var photoUploadResult = await _photoAccessor.AddPhoto(request.File);
            var korisnik = await _context.Users.Include(p => p.Slike)
                .FirstOrDefaultAsync(x => x.UserName == _korisnikAccessor.GetUsername());

            var photo = new Photo
            {
                Url = photoUploadResult.Url,
                Id = photoUploadResult.PublicId
            };

            if (!korisnik.Slike.Any(x => x.IsMain)) photo.IsMain = true;

            korisnik.Slike.Add(photo);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Result<Photo>.Success(photo);

            return Result<Photo>.Failure("Problem adding photo");
        }
    }
}