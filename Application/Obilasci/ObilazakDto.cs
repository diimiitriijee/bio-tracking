using Application.Profiles;
using Domain;

namespace Application.Obilasci;

public class ObilazakDto
{
    public Guid ID { get; set; }
    public string Naziv { get; set; }
    public string Opis { get; set; }
    public DateTime DatumOdrzavanja { get; set; }
    public int BrojMaxPolaznika { get; set; }
    public string MestoOkupljanja { get; set; }
    public ICollection<Komentar> Komentari { get; set; }
    public string Alergije { get; set; }
    public ProfileVodic Vodic { get; set; }
    //public Podrucje Podrucje { get; set; }
    //public ICollection<Profile> Ucesnici { get; set; }
    public ICollection<UcesnikDto> Ucesnici { get; set; } = [];
    public Ruta Ruta {get; set;}
    // Koje sve atribute treba ovde da stavim ???
}