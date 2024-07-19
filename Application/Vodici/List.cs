using Application.Core;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Vodici;

public class List
{
    public class Query : IRequest<Result<List<ProfileVodic>>> {}

    public class Handler : IRequestHandler<Query, Result<List<ProfileVodic>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<Result<List<ProfileVodic>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Result<List<ProfileVodic>>.Success(await _context.Vodici
                .ProjectTo<ProfileVodic>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken));
        }
    }
}