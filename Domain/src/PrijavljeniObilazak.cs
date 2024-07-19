using System.Text.Json.Serialization;

namespace Domain
{
    public class PrijavljeniObilazak
    {
        public Guid ObilazakID { get; set; }
        public Guid KorisnikID { get; set; }
        [JsonIgnore]
        public Obilazak Obilazak { get; set; }
        [JsonIgnore]
        public Korisnik Korisnik { get; set; }
        public DateTime DatumPrijave { get; set; }
    }
}