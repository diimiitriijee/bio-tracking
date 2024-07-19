using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Pratioci
{
    public class List
    {
        public class Query : IRequest<Result<List<Profiles.ProfileVodic>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Profiles.ProfileVodic>>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;
            private readonly IKorisnikAccessor _userAccessor;

            public Handler(DataContext context, IMapper mapper, IKorisnikAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _mapper = mapper;
            }

            public async Task<Result<List<Profiles.ProfileVodic>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Profiles.ProfileVodic>();

                switch (request.Predicate)
                {
                    case "followers"://vracamo one profile koji prate vodica
                        profiles = await _context.UserFollowings.Where(x => x.Target.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})//ovde prosledjujemo currentUsername da bismo znali da li pratimo taj profil kada ga ucitamo
                            .ProjectTo<Profiles.ProfileVodic>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
                            .ToListAsync();
                        break;
                    case "following"://vracamo vodice koje prati taj korisnik
                        profiles = await _context.UserFollowings.Where(x => x.Observer.UserName == request.Username)
                            .Select(u => u.Target)
                            //.ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})//ukljuci ako se nesto buni sad sam dodao sad ce ga probam 
                            .ProjectTo<Profiles.ProfileVodic>(_mapper.ConfigurationProvider, new {currentUsername = _userAccessor.GetUsername()})
                            .ToListAsync();
                        break;
                }

                return Result<List<Profiles.ProfileVodic>>.Success(profiles);
            }
        }
    }
}