using System.Text.Json.Serialization;

namespace Domain
{
    public class KoordinateRute
    {
        public Guid ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [JsonIgnore]
        public Ruta Ruta { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}