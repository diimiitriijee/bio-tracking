using Application.Core;
using Domain;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Rute;

public class Details
{
    public class Query : IRequest<Result<Ruta>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Ruta>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Ruta>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ruta = await _context.Rute
                .Include(r => r.Koordinate.OrderBy(k => k.CreatedAt))
                .SingleOrDefaultAsync(r => r.ID == request.Id);
            return Result<Ruta>.Success(ruta);
        }
    }
}
