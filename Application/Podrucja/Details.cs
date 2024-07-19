using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class Details
{
    public class Query : IRequest<Result<Podrucje>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Podrucje>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Podrucje>> Handle(Query request, CancellationToken cancellationToken)
        {
            var podrucje = await _context.Podrucja
                .Include(p => p.Koordinate.OrderBy(k => k.CreatedAt))
                .Include(o => o.Biljke)
                .ThenInclude(b => b.Biljka)
                .SingleOrDefaultAsync(p => p.ID == request.Id);
                //.FindAsync(request.Id);
            return Result<Podrucje>.Success(podrucje);
        }
    }
}
