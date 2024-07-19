using Domain;
using Domain.src;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence;
public class Seed
{
    public static async Task SeedData(DataContext context)
    {
        if (context.Obilasci.Any()) return;
        
        var obilasci = new List<Obilazak>
        {
            new Obilazak
            {
                DatumOdrzavanja = DateTime.UtcNow.AddMonths(2),
                BrojMaxPolaznika = 10,
            },
            new Obilazak
            {
                DatumOdrzavanja = DateTime.UtcNow.AddMonths(1),
                BrojMaxPolaznika = 20,
            }
        };

        await context.Obilasci.AddRangeAsync(obilasci);
        await context.SaveChangesAsync();
    }

    public static async Task SeedUsers(UserManager<Korisnik> userManager, RoleManager<AppRole> roleManager)
    {
        if(!userManager.Users.Any())
        {
            var korisnici = new List<Korisnik> {
                new Admin{Ime = "Vukan", Prezime = "Taskov", UserName = "tasko_", Email = "taskov.vukan@elfak.rs", LockoutEnabled = false},
                new Korisnik{Ime = "Dimitrije", Prezime = "Najdanovic", UserName = "dika", Email = "dikadika@elfak.rs", LockoutEnabled = false},
                new Vodic{Ime = "Aleksandar", Prezime = "Djordjevic", UserName = "suki", Email = "sukisuki@elfak.rs", BrojOdrzanihObilazaka = 0, LockoutEnabled = false}
            };


            var roles = new List<AppRole> {
                new AppRole{Name = "ObicanKorisnik"},
                new AppRole{Name = "TuristickiVodic"},
                new AppRole{Name = "Administrator"}
            };

            foreach (var role in roles){
                await roleManager.CreateAsync(role);//da kreiramo ulogu u bazi
            }

            foreach(var korisnik in korisnici){
                await userManager.CreateAsync(korisnik, "PrejaK@s1fra");//da kreiramo korisnika sa sifrom u bazi
                    if (korisnik.Ime == "Vukan")
                        await userManager.AddToRoleAsync(korisnik, "Administrator");
                    if (korisnik.Ime == "Dimitrije")
                        await userManager.AddToRoleAsync(korisnik, "ObicanKorisnik");
                    if (korisnik.Ime == "Aleksandar")
                        await userManager.AddToRoleAsync(korisnik, "TuristickiVodic");
            }
        }
    }
}