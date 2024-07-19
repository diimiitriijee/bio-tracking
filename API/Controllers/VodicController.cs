using Application.Vodici;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize(Policy = "RequireTuristickiVodicRole")]
public class VodicController : BaseApiController
{
    [HttpGet("VratiSveVodice")]
    public async Task<IActionResult> VratiVodice()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    //[Authorize(Roles = "ObicanKorisnik")]//ovaj nacin nece da se koristi na kraju nego sa policy, da zamenis sve
    [HttpGet("VratiVodica/{id}")]
    public async Task<IActionResult> VratiVodica(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    [HttpGet("VratiProfil/{username}")]
    public async Task<IActionResult> VratiProfil(string username)
    {
        return HandleResult(await Mediator.Send(new DetailsByUsername.Query { Username = username }));
    }

    // public async Task<IActionResult> IzmeniVodica(Guid id, Vodic vodic)
    // {
    //     vodic.Id = id;
    //     return HandleResult(await Mediator.Send(new Edit.Command {Vodic = vodic}));
    // }

    [HttpPut("IzmeniProfilVodica")]
    public async Task<IActionResult> IzmeniVodica(string ime, string prezime, string telefon, DateTime datumRodjenja, string strucnaSprema)
    {
        return HandleResult(await Mediator.Send(new Edit.Command {
            Ime = ime, Prezime = prezime, Telefon = telefon, DatumRodjenja = datumRodjenja, StrucnaSprema = strucnaSprema}));
    }

    // [HttpPut("IzmeniProfilVodica")]
    // public async Task<IActionResult> IzmeniVodica(ProfileVodic vodic)
    // {
    //     return HandleResult(await Mediator.Send(new Edit.Command {Vodic = vodic}));
    // }

    [HttpDelete("ObrisiVodica/{id}")]
    public async Task<IActionResult> ObrisiVodica(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    }

    [HttpGet("OcenaVodica/{id}")]
    public async Task<IActionResult> OcenaVodica(Guid id)
    {
        return HandleResult(await Mediator.Send(new ProsecnaOcena.Query{Id = id}));
    }
}