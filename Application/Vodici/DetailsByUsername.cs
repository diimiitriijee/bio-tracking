using Application.Core;
using Application.Interfaces;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Vodici;
public class DetailsByUsername
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
            var user = await _context.Vodici
                .ProjectTo<ProfileVodic>(_mapper.ConfigurationProvider, new { currentUsername = _korisnikAccessor.GetUsername()})
                .SingleOrDefaultAsync(x => x.Username == request.Username);

            if (user == null) return Result<ProfileVodic>.Failure("Nije pronadjen vodic sa takvim username-om");;

            return Result<ProfileVodic>.Success(user);
        }
    }
}