using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Ocena
    {
        public Guid ID { get; set; }
        public Korisnik Korisnik { get; set; }
        [JsonIgnore]
        public Vodic Vodic { get; set; }
        [Range(1, 5, ErrorMessage = "Vrednost ocene mora biti između 1 i 5.")]
        public int VrednostOcene { get; set; }
        public string Komentar { get; set; }
    }
}