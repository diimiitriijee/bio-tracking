using System.Text.Json.Serialization;

namespace Domain
{
    public class Ruta
    {
        public Guid ID { get; set; }
        public string Opis { get; set; }
        public double Duzina { get; set; }
        public string Tip { get; set; }
        public bool ZaDecu { get; set; }
        public string Prohodnost { get; set; }
        public string Uspon { get; set; }
        public ICollection<KoordinateRute> Koordinate { get; set; } = new List<KoordinateRute>();
        [JsonIgnore]//privremeno resenje dok ne napravim DTO
        public Podrucje Podrucje {get; set;}
    }
}