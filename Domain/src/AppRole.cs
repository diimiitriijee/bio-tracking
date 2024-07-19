using Microsoft.AspNetCore.Identity;

namespace Domain.src
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<KorisnikRole> KorisnikRoles { get; set; } //veza prema klasi Korisnik

    }
}