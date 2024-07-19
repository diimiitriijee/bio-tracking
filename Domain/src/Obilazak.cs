namespace Domain;

public class Obilazak
{
    public Guid ID { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public DateTime DatumOdrzavanja { get; set; }
    public int BrojMaxPolaznika { get; set; }
    public string MestoOkupljanja { get; set; }
    public ICollection<Komentar> Komentari { get; set; } = new List<Komentar>();
    public string Alergije { get; set; }
    public Vodic Vodic { get; set; }
    public ICollection<PrijavljeniObilazak> Ucesnici { get; set; } = new List<PrijavljeniObilazak>();
    public Podrucje Podrucje { get; set; }
    public Ruta Ruta {get; set;}
}