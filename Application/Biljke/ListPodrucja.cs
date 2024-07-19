using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Biljke;

public class ListPodrucja
{
    public class Query : IRequest<Result<List<Podrucje>>>
    {
        public Guid BiljkaId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Podrucje>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<List<Podrucje>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var biljka = await _context.Biljke
                .Include(b => b.Podrucja)
                .ThenInclude(pb => pb.Podrucje)
                .ThenInclude(p => p.Koordinate.OrderBy(k => k.CreatedAt))
                .FirstOrDefaultAsync(b => b.ID == request.BiljkaId);

            if (biljka == null)
                return Result<List<Podrucje>>.Failure("Biljka nije pronađena");

            var podrucja = biljka.Podrucja.Select(pb => pb.Podrucje).ToList();

            return Result<List<Podrucje>>.Success(podrucja);
        }
    }
}