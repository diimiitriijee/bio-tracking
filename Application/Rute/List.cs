using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Rute;

public class List
{
    public class Query : IRequest<Result<List<Ruta>>> {}

    public class Handler : IRequestHandler<Query, Result<List<Ruta>>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Ruta>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Result<List<Ruta>>.Success(await _context.Rute
                .Include(p => p.Koordinate.OrderBy(k => k.CreatedAt))
                .ToListAsync());
        }
    }
}