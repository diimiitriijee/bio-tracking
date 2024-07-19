namespace Domain.src;

public class VodicZahtev
{
    public Guid Id { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Telefon { get; set; }
    public DateTime DatumRodjenja { get; set; }
    public string StrucnaSprema { get; set; }
    public int BrojOdrzanihObilazaka { get; set; }
    public string PutanjaSlikeDiplome { get; set; }
}