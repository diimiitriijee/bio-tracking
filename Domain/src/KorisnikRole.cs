//ova klasa predstavlja vezu izmedju tipova korisnika aplikacije i uloga u aplikaciji (appRole)
using Microsoft.AspNetCore.Identity;

namespace Domain.src
{
    public class KorisnikRole :IdentityUserRole<Guid>
    {
        public Korisnik Korisnik { get; set; }
        public AppRole Role { get; set; }
    }
}