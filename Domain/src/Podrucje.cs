using System.Text.Json.Serialization;

namespace Domain;

public class Podrucje
{
    public Guid ID { get; set; }
    public string Oblast { get; set; }
    public ICollection<KoordinatePodrucja> Koordinate { get; set; } = new List<KoordinatePodrucja>();
    //[JsonIgnore]
    public ICollection<Podrucja_Biljke> Biljke { get; set; } = new List<Podrucja_Biljke>();
    [JsonIgnore]
    public ICollection<Obilazak> Obilasci { get; set; } = new List<Obilazak>();
    public ICollection<Ruta> Rute {get; set; } = [];

}