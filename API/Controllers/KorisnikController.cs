using Application.Korisnici;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class KorisnikController : BaseApiController // potrebne izmene funkcija
{
    [Authorize(Policy = "RequireAdministratorRole")]
    //OVU FUNKCIJU BRISI POSLE KAD DIKA ODRADI AUTH JER OVDE NE TREBA DA POSTOJI PREBACENA JE U ADMINA, ISTO VAZI ZA DELETEKORISNIK I CREATEKORISNIK
    [HttpGet("VratiSveKorisnike")]
    public async Task<IActionResult> VratiKorisnike()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    [Authorize(Policy = "RequireObicanKorisnikRole")]
    [HttpGet("VratiKorisnika/{id}")]
    public async Task<IActionResult> VratiKorisnika(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    [HttpPost("KreirajKorisnika")]//ova je visak skroz obrisi posle neka je za sad
    public async Task<IActionResult> KreirajKorisnika(Korisnik korisnik)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Korisnik = korisnik}));
    }

    [HttpPut("IzmeniKorisnika/{id}")]
    public async Task<IActionResult> IzmeniKorisnika(Guid id, Korisnik korisnik)
    {
        korisnik.Id = id;
        return HandleResult(await Mediator.Send(new Edit.Command {Korisnik = korisnik}));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("ObrisiKorisnika/{id}")]//ovu prebacujem u adminController ali namerno za sad komentarisem samo ovde zbog diku
    public async Task<IActionResult> ObrisiKorisnika(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("ObrisiRoleKorisnika/{id}")]
    public async Task<IActionResult> ObrisiRoleKorisnika(Guid id)
    {
        return HandleResult(await Mediator.Send(new DeleteRoles.Command {Id = id}));
    }

    //TODO:
    //UcesniciObilaska
    //TuristickiVodicObilaska?
    //ObrisiKorisnika u adminController
    //TuristickiVodic Controller
}