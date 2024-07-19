using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.Biljke;

public class BiljkaDto
{
    //public Guid ID { get; set; }
    [Required]
    public string Naziv { get; set; }
    [Required]
    public string Opis { get; set; }
    [Required]
    public string Vrsta { get; set; }
    [Required]
    public bool Lekovita { get; set; }
    [Required]
    public IFormFile Slika { get; set; }
}