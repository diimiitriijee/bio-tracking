using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Biljke;

public class List
{
    public class Query : IRequest<Result<List<Biljka>>> 
    {

    }

    public class Handler : IRequestHandler<Query, Result<List<Biljka>>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }
        public async Task<Result<List<Biljka>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return Result<List<Biljka>>.Success(await _context.Biljke.ToListAsync(cancellationToken));
        }
    }
}