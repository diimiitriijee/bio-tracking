using Application.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    //var podrucje = await _context.Podrucja.FindAsync(request.Id);
                    var podrucje = await _context.Podrucja
                        .Include(p => p.Koordinate)
                        .Include(p => p.Biljke)
                        .Include(p => p.Obilasci)
                        .FirstOrDefaultAsync(p => p.ID == request.Id);
                    if (podrucje == null) return Result<Unit>.Failure("Failed to find the podrucje");
                    // Obriši Podrucje
                    _context.Remove(podrucje);

                    // Sačuvaj promene
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;
                    if (!result) 
                    {
                        return Result<Unit>.Failure("Failed to delete the podrucje");
                    }
                    return Result<Unit>.Success(Unit.Value);
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.Message ?? ex.Message;
                    return Result<Unit>.Failure($"An error occurred: {innerException}, {ex.StackTrace}, {ex.Source}, {ex.TargetSite}, {ex.InnerException.StackTrace}, {ex.InnerException.Data}");
                }
                catch (Exception ex)
                {
                    return Result<Unit>.Failure($"An unexpected error occurred: {ex.Message}");
                }
            }
            // DA LI TREBA BRISANJE DA SE RESI U DATACONTEXT POMOCU CASCADE A NE OVDE DA SE BRISE SVE RUCNO!!!
            // public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            // {
            //     using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            //     try
            //     {
            //         // Pronađi Podrucje
            //         var podrucje = await _context.Podrucja.FindAsync(request.Id);
            //         if (podrucje == null) return Result<Unit>.Failure("Failed to find the podrucje");

            //         // Pronađi i obriši sve zapise u Podrucja_Biljke koji se odnose na ovo Podrucje
            //         var biljkeNaPodrucju = await _context.Podrucja_Biljke
            //             .Where(pb => pb.PodrucjeID == podrucje.ID)
            //             .ToListAsync(cancellationToken);

            //         if (biljkeNaPodrucju.Any())
            //         {
            //             _context.Podrucja_Biljke.RemoveRange(biljkeNaPodrucju);
            //         }

            //         // Pronađi i obriši sve zapise u KoordinatePodrucja koji se odnose na ovo Podrucje
            //         var koordinatePodrucja = await _context.KoordinatePodrucja
            //             .Where(kp => kp.Podrucje.ID == podrucje.ID)
            //             .ToListAsync(cancellationToken);

            //         if (koordinatePodrucja.Any())
            //         {
            //             _context.KoordinatePodrucja.RemoveRange(koordinatePodrucja);
            //         }

            //         // Pronađi i obriši sve zapise u Obilasci koji se odnose na ovo Podrucje
            //         var obilasciNaPodrucju = await _context.Obilasci
            //             .Where(o => o.Podrucje.ID == podrucje.ID)
            //             .ToListAsync(cancellationToken);

            //         if (obilasciNaPodrucju.Any())
            //         {
            //             foreach (var obilazak in obilasciNaPodrucju)
            //             {
            //                 // Pronađi i obriši sve prijavljene učesnike za svaki obilazak
            //                 var prijavljeniUcesnici = await _context.PrijavljeniObilasci
            //                     .Where(po => po.ObilazakID == obilazak.ID)
            //                     .ToListAsync(cancellationToken);

            //                 if (prijavljeniUcesnici.Any())
            //                 {
            //                     _context.PrijavljeniObilasci.RemoveRange(prijavljeniUcesnici);
            //                 }

            //                 // Pronađi i obriši sve komentare za svaki obilazak
            //                 var komentariObilaska = await _context.Komentari
            //                     .Where(k => k.Obilazak.ID == obilazak.ID)
            //                     .ToListAsync(cancellationToken);

            //                 if (komentariObilaska.Any())
            //                 {
            //                     _context.Komentari.RemoveRange(komentariObilaska);
            //                 }
            //             }

            //             _context.Obilasci.RemoveRange(obilasciNaPodrucju);
            //         }

            //         // Obriši Podrucje
            //         _context.Podrucja.Remove(podrucje);

            //         // Sačuvaj promene
            //         var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            //         if (!result) 
            //         {
            //             await transaction.RollbackAsync(cancellationToken);
            //             return Result<Unit>.Failure("Failed to delete the podrucje");
            //         }

            //         await transaction.CommitAsync(cancellationToken);
            //         return Result<Unit>.Success(Unit.Value);
            //     }
            //     catch (DbUpdateException ex)
            //     {
            //         await transaction.RollbackAsync(cancellationToken);
            //         var innerException = ex.InnerException?.Message ?? ex.Message;
            //         return Result<Unit>.Failure($"An error occurred: {innerException}");
            //     }
            //     catch (Exception ex)
            //     {
            //         await transaction.RollbackAsync(cancellationToken);
            //         return Result<Unit>.Failure($"An unexpected error occurred: {ex.Message}");
            //     }
            //}
        }
    }
}
