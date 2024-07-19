using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Rute
{
    public class ListaRutaPodrucja
    {
        public class Query : IRequest<Result<PagedList<Ruta>>> 
        {
            public Guid PodrucjeId { get; set; }
            public PagingParams PagingParams {get; set;}
        }

        public class Handler : IRequestHandler<Query, Result<PagedList<Ruta>>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }
            public async Task<Result<PagedList<Ruta>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var podrucje = await _context.Podrucja.FindAsync(request.PodrucjeId);
                if (podrucje == null) return Result<PagedList<Ruta>>.Failure("Podrucje nije pronadjeno");

                var query =  _context.Rute.Include(r => r.Koordinate.OrderBy(k => k.CreatedAt)).Where(r => r.Podrucje == podrucje).AsQueryable();
                return Result<PagedList<Ruta>>.Success(await PagedList<Ruta>.CreateAsync(query, request.PagingParams.PageNumber, request.PagingParams.PageSize));
            }
        }
    }
}