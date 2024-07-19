namespace Application.Podrucja;

public class EditKoordinateDto
{
    //public Guid PodrucjeId { get; set; }
    public List<KoordinateDto> Koordinate { get; set; } = new List<KoordinateDto>();
}

public class KoordinateDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}