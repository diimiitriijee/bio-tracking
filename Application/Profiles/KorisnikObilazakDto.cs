using System.Text.Json.Serialization;

namespace Application.Profiles;

public class KorisnikObilazakDto
{
    public Guid ID { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public DateTime DatumOdrzavanja { get; set; }
    public string VodicUsername { get; set; }
}