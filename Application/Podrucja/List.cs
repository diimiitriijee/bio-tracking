using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class List
{
    public class Query : IRequest<Result<List<Podrucje>>> {}

    public class Handler : IRequestHandler<Query, Result<List<Podrucje>>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Podrucje>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Result<List<Podrucje>>.Success(await _context.Podrucja
                                                        .Include(p => p.Koordinate.OrderBy(k => k.CreatedAt))
                                                        .Include(o => o.Biljke)
                                                        .ThenInclude(b => b.Biljka)
                                                        .ToListAsync());
        }
    }
}