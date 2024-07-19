using Application.Core;
using Application.Obilasci;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ObilazakController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("VratiSveObilaske")]
    public async Task<IActionResult> VratiObilaske([FromQuery]ObilazakParams parametri)
    {
        return HandlePagedResult(await Mediator.Send(new List.Query{Params = parametri}));//ovde vracamo paged rezultate za prikaz obilazaka, seti se da dodas ovo i za korisnike ako ga i tu odradis
    }

    [AllowAnonymous]
    [HttpGet("VratiObilazak/{id}")]
    public async Task<IActionResult> VratiObilazak(Guid id)
    {
        return HandleResult(await Mediator.Send(new Application.Obilasci.Details.Query{Id = id}));
    }
    
    [AllowAnonymous]
    [HttpGet("VratiObilaskePodrucja/{idPodrucja}")]
    public async Task<IActionResult> VratiObilaskePodrucja(Guid idPodrucja, [FromQuery]ObilazakParams parametri)
    {
        return HandlePagedResult(await Mediator.Send(new ObilasciNaPodrucju.Query{IdPodrucja = idPodrucja, Params = parametri}));
    }
    
    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("KreirajObilazak/{idPodrucja}/{idRute}")]
    public async Task<IActionResult> KreirajObilazak(Obilazak obilazak, Guid idPodrucja, Guid idRute)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Obilazak = obilazak, IdPodrucja = idPodrucja, IdRute = idRute}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniObilazak/{id}")]
    public async Task<IActionResult> IzmeniObilazak(Guid id, Obilazak obilazak)
    {
        obilazak.ID = id;
        return HandleResult(await Mediator.Send(new Application.Obilasci.Edit.Command {Obilazak = obilazak}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpDelete("ObrisiObilazak/{id}")]
    public async Task<IActionResult> ObrisiObilazak(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [Authorize(Policy = "RequireObicanKorisnikRole")]
    [HttpPost("DodajUcesnika/{idObilaska}")]// post ili put
    public async Task<IActionResult> DodajUcesnika(Guid idObilaska)
    {
        return HandleResult(await Mediator.Send(new DodajUcesnika.Command {IdObilaska = idObilaska}));
    }

    [Authorize(Policy = "RequireObicanKorisnikRole")]
    [HttpDelete("UkloniUcesnika/{idObilaska}")]
    public async Task<IActionResult> UkloniUcesnika(Guid idObilaska)
    {
        return HandleResult(await Mediator.Send(new UkloniUcesnika.Command {IdObilaska = idObilaska}));
    }

    [AllowAnonymous]
    [HttpGet("VratiUcesnike/{idObilaska}")]
    public async Task<IActionResult> VratiUcesnike(Guid idObilaska)
    {
        return HandleResult(await Mediator.Send(new ListUcesnika.Query { IdObilaska = idObilaska }));
    }

    [Authorize]
    [HttpGet("VratiVodica/{idObilaska}")]
    public async Task<IActionResult> VratiVodica(Guid idObilaska)
    {
        return HandleResult(await Mediator.Send(new VodicObilaska.Query { IdObilaska = idObilaska }));
    }

    [Authorize(Policy = "RequireObicanKorisnikRole")]
    [HttpPost("OceniVodica/{idObilaska}")]
    public async Task<IActionResult> OceniVodica(Guid idObilaska, OcenaModel ocena)
    {
        return HandleResult(await Mediator.Send(new DodajOcenuVodicu.Command {IdObilaska = idObilaska, VrednostOcene = ocena.VrednostOcene, Komentar = ocena.Komentar}));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost("DodajRutuObilasku/{idObilaska}/{idRute}")]// post ili put?
    public async Task<IActionResult> DodajRutuObilasku(Guid idObilaska, Guid idRute)
    {
        return HandleResult(await Mediator.Send(new DodajRutuObilasku.Command { ObilazakId = idObilaska, RutaId = idRute }));
    }
}