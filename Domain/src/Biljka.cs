using System.Text.Json.Serialization;
using Domain.src;

namespace Domain;

public class Biljka
{
    public Guid ID { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public string Vrsta { get; set; }
    public bool Lekovita { get; set; }
    public string Slika { get; set; }
    [JsonIgnore]
    public ICollection<Podrucja_Biljke> Podrucja { get; set; } = new List<Podrucja_Biljke>();
}