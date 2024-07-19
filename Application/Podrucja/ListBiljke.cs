using Application.Biljke;
using Application.Core;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class ListBiljke
{
    public class Query : IRequest<Result<PagedList<Biljka>>>
    {
        public Guid PodrucjeId { get; set; }
        public BiljkeParams Params { get; set; } // Parametri za lekovitost i paginiranje
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<Biljka>>>
    {
        private readonly DataContext _context;

        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<PagedList<Biljka>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // var podrucje = await _context.Podrucja
            //     .Include(p => p.Biljke)
            //     .ThenInclude(pb => pb.Biljka)
            //     .FirstOrDefaultAsync(p => p.ID == request.PodrucjeId);

            // if (podrucje == null) return Result<PagedList<Biljka>>.Failure("Podrucje not found");

            // var biljkeQuery = podrucje.Biljke.Select(pb => pb.Biljka).AsQueryable();

            // // Paginacija
            // var pagedBiljke = await PagedList<Biljka>.CreateAsync(biljkeQuery, request.Params.PageNumber, request.Params.PageSize);

            // return Result<PagedList<Biljka>>.Success(pagedBiljke);
            
            var query = _context.Biljke
                .Where(b => b.Podrucja.Any(p => p.PodrucjeID == request.PodrucjeId))
                .OrderBy(b => b.Naziv)
                .AsQueryable();
            if (request.Params.Lekovita && !request.Params.NeLekovita)
            {
                query = query.Where(b => b.Lekovita == true);
            }
            else if (request.Params.NeLekovita && !request.Params.Lekovita)
            {
                query = query.Where(b => b.Lekovita == false);
            }

            var pagedBiljke = await PagedList<Biljka>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize);
            return Result<PagedList<Biljka>>.Success(pagedBiljke);
        }
    }
}