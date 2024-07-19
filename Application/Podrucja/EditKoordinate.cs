using Application.Core;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Podrucja;

public class EditKoordinate
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid IdPodrucja { get; set; }
        public List<KoordinateDto> EditKoordinateDto { get; set; }
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
            var podrucje = await _context.Podrucja
                .Include(p => p.Koordinate)
                .FirstOrDefaultAsync(p => p.ID == request.IdPodrucja);

            if (podrucje == null) return Result<Unit>.Failure("Podrucje not found");

            // Remove existing coordinates
            //_context.KoordinatePodrucja.RemoveRange(podrucje.Koordinate);
            // foreach (var koordinate in podrucje.Koordinate)
            // {
            //     _context.KoordinatePodrucja.Remove(koordinate);
            // }
            var num = podrucje.Koordinate.Count();
            if (request.EditKoordinateDto.Count() > num)
                return Result<Unit>.Failure("Mozete izmeniti samo postojece koordinate.");
                
            var br = 0;
            var niz = podrucje.Koordinate.ToArray();
            foreach (var koordinateDto in request.EditKoordinateDto)
            {
                niz[br].Latitude = koordinateDto.Latitude;
                niz[br].Longitude = koordinateDto.Longitude;
                br++;
            }

            try
            {
                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to update coordinates");
                }

                return Result<Unit>.Success(Unit.Value);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency exception
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Podrucje || entry.Entity is KoordinatePodrucja)
                    {
                        var databaseEntity = await _context.Entry(entry.Entity)
                            .GetDatabaseValuesAsync(cancellationToken);

                        if (databaseEntity == null)
                        {
                            return Result<Unit>.Failure($"{entry.Entity.GetType().Name} not found in the database.");
                        }

                        // Update the original values to reflect the current values in the database.
                        entry.OriginalValues.SetValues(databaseEntity);
                    }
                    else
                    {
                        return Result<Unit>.Failure($"Concurrency issue detected for a different entity type: {entry.Entity.GetType().Name}");
                    }
                }

                try
                {
                    var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                    if (!result)
                    {
                        return Result<Unit>.Failure("Failed to update coordinates after concurrency resolution.");
                    }

                    return Result<Unit>.Success(Unit.Value);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Result<Unit>.Failure("Failed to resolve concurrency issue.");
                }
            }
            catch (Exception ex)
            {
                return Result<Unit>.Failure($"An error occurred: {ex.Message}");
            }

            // var result = await _context.SaveChangesAsync() > 0;

            // if (!result) return Result<Unit>.Failure("Failed to update coordinates");

            // return Result<Unit>.Success(Unit.Value);
        }
    }
}