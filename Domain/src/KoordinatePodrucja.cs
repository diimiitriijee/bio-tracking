using System.Text.Json.Serialization;

namespace Domain
{
    public class KoordinatePodrucja
    {
        public Guid ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore]
        public Podrucje Podrucje { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}