using Domain.src;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class Korisnik : IdentityUser<Guid>
{
    //public Guid ID { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Telefon { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public ICollection<Photo> Slike { get; set; }
    public ICollection<PrijavljeniObilazak> PrijavljeniObilasci { get; set; } = new List<PrijavljeniObilazak>();
    public ICollection<KorisnikRole> KorisnikRoles { get; set; } = []; //veza prema ulogama koje moze da ima
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<UserFollowing> Followings {get; set; } = new List<UserFollowing>();
    public ICollection<UserFollowing> Followers {get; set;} = new List<UserFollowing>();
}