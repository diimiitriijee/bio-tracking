using Application.Core;
using Application.Profiles;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Persistence;

namespace Application.Vodici;

public class Details
{
    public class Query : IRequest<Result<ProfileVodic>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ProfileVodic>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public Handler(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<Result<ProfileVodic>> Handle(Query request, CancellationToken cancellationToken)
        {
            var vodic = await _context.Vodici.FindAsync(request.Id);
            return Result<ProfileVodic>.Success(_mapper.Map<ProfileVodic>(vodic));
        }
    }
}
