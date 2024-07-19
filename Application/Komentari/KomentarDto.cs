namespace Application.Komentari;

public class KomentarDto
{
    public Guid ID { get; set; }
    public string Tekst { get; set; }
    public DateTime DatumKreiranja { get; set; }
    public string Username { get; set; }
    public string Ime { get; set; }
    public string Prezime { get; set; }
    public string SlikaProfila { get; set; }
}