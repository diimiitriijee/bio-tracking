using System.Text.Json.Serialization;

namespace Domain
{
    public class Komentar
    {
        public Guid ID { get; set; }
        public string Tekst { get; set; }
        public DateTime DatumKreiranja { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public Korisnik Korisnik { get; set; }
        [JsonIgnore]
        public Obilazak Obilazak { get; set; }
    }
}