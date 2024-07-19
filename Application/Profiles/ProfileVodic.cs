using Domain;

namespace Application.Profiles;

public class ProfileVodic : Profile
{
    public string StrucnaSprema { get; set; }
    public int BrojOdrzanihObilazaka { get; set; }
    public ICollection<Ocena> Ocene { get; set; }
    public double ProsecnaOcena { get; set; }
    public int FollowersCount { get; set; }
    public bool Following {get; set;}
}