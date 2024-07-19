using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterVodicDto 
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Ime { get; set; }
    [Required]
    public string Prezime { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Telefon { get; set; }

    [Required]
    public DateTime DatumRodjenja { get; set; }
    [Required]
    public string StrucnaSprema { get; set; }
    //[Required]
    public IFormFile SlikaDiplome { get; set; }
}