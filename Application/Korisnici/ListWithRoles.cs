// using Application.Core;
// using Domain;
// using MediatR;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Persistence;

// namespace Application.Korisnici
// {
//     public class ListWithRoles
//     {
//         public class Query : IRequest<Result<List<Korisnik>>> {}

//         public class Handler : IRequestHandler<Query, Result<List<Korisnik>>>
//         {
//             private readonly DataContext _context;
//             private readonly UserManager<Korisnik> _userManager;    
//             public Handler(DataContext context, UserManager<Korisnik> userManager)
//             {
//                 _userManager = userManager;
//                 _context = context;
//             }
//             public async Task<Result<List<Korisnik>>> Handle(Query request, CancellationToken cancellationToken)
//             {
//                 var korisnici = await _userManager.Users.OrderBy(korisnik => korisnik.UserName).Select(korisnik => new  {
//                     korisnik.Id,
//                     Username = korisnik.UserName,
//                     Roles = korisnik.KorisnikRoles.Select(role => role.Role).ToList()
//                 }).ToListAsync();

//                 return Result<List<Korisnik>>.Success(korisnici);
//             }
//         }
//     }
// }