using Application.Core;
using Application.Rute;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class RutaController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("VratiSveRute")]
    public async Task<IActionResult> VratiSveRute()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    [AllowAnonymous]
    [HttpGet("VratiRutePodrucja/{idPodrucja}")]
    public async Task<IActionResult> VratiRutePodrucja(Guid idPodrucja, [FromQuery]PagingParams Params)
    {
        return HandlePagedResult(await Mediator.Send(new ListaRutaPodrucja.Query{PodrucjeId = idPodrucja, PagingParams = Params}));
    }

    [AllowAnonymous]
    [HttpGet("VratiRutu/{id}")]
    public async Task<IActionResult> VratiRutu(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("KreirajRutu/{idPodrucja}")]
    public async Task<IActionResult> KreirajRutu(Ruta ruta, Guid idPodrucja)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Ruta = ruta, PodrucjeId = idPodrucja}));
    }
    
    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniRutu/{id}")]
    public async Task<IActionResult> IzmeniRutu(Guid id, Ruta ruta)
    {
        return HandleResult(await Mediator.Send(new Edit.Command {Ruta = ruta}));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("ObrisiRutu/{id}")]
    public async Task<IActionResult> ObrisiPodrucje(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("DodajKoordinate/{idRute}")]// post ili put?
    public async Task<IActionResult> DodajKoordinate(Guid idRute, double latitude, double longitude)
    {
        return HandleResult(await Mediator.Send(new DodajKoordinate.Command { RutaId = idRute, Latitude = latitude, Longitude = longitude }));
    }

    [AllowAnonymous]
    [HttpGet("VratiKoordinate/{idRute}")]
    public async Task<IActionResult> VratiKoordinate(Guid idRute)
    {
        return HandleResult(await Mediator.Send(new ListKoordinate.Query { RutaId = idRute }));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("DodajKoordeRuti/{idRute}")]
    public async Task<IActionResult> DodajKoordeRute(Guid idRute, [FromBody]KoordinataRute[] koordinate)
    {
        return HandleResult(await Mediator.Send(new DodajKoorde.Command { RutaId = idRute, Koordinate = koordinate }));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniKoordeRute/{idRute}")]
    public async Task<IActionResult> IzmeniKoordeRute(Guid idRute, [FromBody]KoordinataRute[] koordinate)
    {
        return HandleResult(await Mediator.Send(new IzmeniKoorde.Command { RutaId = idRute, Koordinate = koordinate }));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("ObrisiKoordeRute/{idRute}")]
    public async Task<IActionResult> IzbrisiKoordeRute(Guid idRute)
    {
        return HandleResult(await Mediator.Send(new ObrisiKoorde.Command { RutaId = idRute }));
    }
}