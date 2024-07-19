using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles;
public class Details
{
    public class Query : IRequest<Result<ProfileVodic>>
    {
        public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ProfileVodic>>
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

        public async Task<Result<ProfileVodic>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Vodici.Include(x => x.Ocene).Include(x => x.Slike).Include(x=>x.Followers).Include(x=>x.Followings)
                .ProjectTo<ProfileVodic>(_mapper.ConfigurationProvider, new { currentUsername = _korisnikAccessor.GetUsername()})
                .SingleOrDefaultAsync(x => x.Username == request.Username);

            if (user == null) {
                var korisnik = await _context.Users.Include(x => x.Slike).Include(x => x.PrijavljeniObilasci).Include(x=>x.Followings).SingleOrDefaultAsync(x => x.UserName == request.Username);
                if (korisnik == null) return Result<ProfileVodic>.Failure("Nije pronadjen korisnik sa time username-om");

                return Result<ProfileVodic>.Success(_mapper.Map<ProfileVodic>(_mapper.Map<Profile>(korisnik)));
            }
            
            return Result<ProfileVodic>.Success(user);
        }
    }
}