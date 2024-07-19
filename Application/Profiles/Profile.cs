using Domain.src;

namespace Application.Profiles;

public class Profile
{
    public string Username { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Telefon { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string SlikaProfila { get; set; }
    public ICollection<Photo> Slike { get; set; }
    public int BrojPrijavljenihObilazaka {get; set;}
    public int FollowingCount { get; set; }
    
    // Dodaj role ukoliko bude bilo problema
    // public ICollection<KorisnikRole> KorisnikRoles { get; set; }
}