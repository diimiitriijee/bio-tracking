using Application.Core;
using Domain;
using MediatR;
using Persistence;

namespace Application.Biljke;

public class Details
{
    public class Query : IRequest<Result<Biljka>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<Biljka>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<Biljka>> Handle(Query request, CancellationToken cancellationToken)
        {
            var biljka = await _context.Biljke.FindAsync(request.Id);
            return Result<Biljka>.Success(biljka);
        }
    }
}
