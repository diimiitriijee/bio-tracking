using Application.Core;
using MediatR;
using Persistence;

namespace Application.Vodici;

public class ProsecnaOcena
{
    public class Query : IRequest<Result<double>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<double>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }

        public async Task<Result<double>> Handle(Query request, CancellationToken cancellationToken)
        {
            var vodic = await _context.Vodici.FindAsync(request.Id);
            if (vodic == null)
                return Result<double>.Failure("Vodič nije pronađen");

            var ocena = vodic.Ocene.Average(o => o.VrednostOcene);

            return Result<double>.Success(ocena);
        }
    }
}