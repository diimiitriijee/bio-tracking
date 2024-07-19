using System.Text.Json.Serialization;

namespace Domain
{
    public class Podrucja_Biljke
    {
        public Guid BiljkaID { get; set; }
        public Guid PodrucjeID { get; set; }
        //[JsonIgnore]
        public Biljka Biljka { get; set; }
        [JsonIgnore]
        public Podrucje Podrucje { get; set; }
    }
}