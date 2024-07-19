using System.Runtime.InteropServices.Marshalling;
using Application.Core;
using Application.Podrucja;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PodrucjeController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("VratiSvaPodrucja")]
    public async Task<IActionResult> VratiPodrucja()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    [AllowAnonymous]
    [HttpGet("VratiPodrucje{id}")]
    public async Task<IActionResult> VratiPodrucje(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("KreirajPodrucje")]
    public async Task<IActionResult> KreirajPodrucje(Podrucje podrucje)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Podrucje = podrucje}));
    }
    
    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniPodrucje{id}")]
    public async Task<IActionResult> IzmeniPodrucje(Guid id, Podrucje podrucje)
    {
        podrucje.ID = id;
        return HandleResult(await Mediator.Send(new Edit.Command {Podrucje = podrucje}));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("ObrisiPodrucje/{id}")]
    public async Task<IActionResult> ObrisiPodrucje(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("DodajKoordinate/{id}")]// post ili put?
    public async Task<IActionResult> DodajKoordinate(Guid id, double latitude, double longitude)
    {
        return HandleResult(await Mediator.Send(new DodajKoordinate.Command { PodrucjeId = id, Latitude = latitude, Longitude = longitude }));
    }

    // [HttpPut("IzmeniKoordinatePodrucja/{id}")]
    // public async Task<IActionResult> IzmeniKoordinatePodrucja(Guid id, EditKoordinateDto editKoordinateDto)
    // {
    //     //editKoordinateDto.PodrucjeId = id;
    //     return HandleResult(await Mediator.Send(new EditKoordinate.Command { IdPodrucja = id, EditKoordinateDto = editKoordinateDto }));
    // }
    
    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniKoordinatePodrucja/{id}")]
    public async Task<IActionResult> IzmeniKoordinatePodrucja(Guid id, List<KoordinateDto> koordinate)
    {
        return HandleResult(await Mediator.Send(new EditKoordinate.Command { IdPodrucja = id, EditKoordinateDto = koordinate }));
    }

    [AllowAnonymous]
    [HttpGet("VratiKoordinate/{id}")]
    public async Task<IActionResult> VratiKoordinate(Guid id)
    {
        return HandleResult(await Mediator.Send(new ListKoordinate.Query { PodrucjeId = id }));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("DodajBiljku/{idPodrucja}/{idBiljke}")]// post ili put?
    public async Task<IActionResult> DodajBiljku(Guid idPodrucja, Guid idBiljke)
    {
        return HandleResult(await Mediator.Send(new DodajBiljku.Command { PodrucjeId = idPodrucja, BiljkaId = idBiljke }));
    }

    [AllowAnonymous]
    [HttpGet("VratiBiljkePodrucja/{id}")]
    public async Task<IActionResult> VratiBiljkePodrucja(Guid id, [FromQuery]Application.Biljke.BiljkeParams parametri)
    {
        return HandlePagedResult(await Mediator.Send(new ListBiljke.Query { PodrucjeId = id, Params = parametri }));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("IzbaciBiljku/{idPodrucja}/{idBiljke}")]
    public async Task<IActionResult> IzbaciBiljku(Guid idPodrucja, Guid idBiljke)
    {
        return HandleResult(await Mediator.Send(new IzbaciBiljku.Command { PodrucjeId = idPodrucja, BiljkaId = idBiljke }));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPost("DodajKoorde/{id}")]
    public async Task<IActionResult> DodajKoorde(Guid id, [FromBody]Koordinata[] koordinate)
    {
        return HandleResult(await Mediator.Send(new DodajKoorde.Command { PodrucjeId = id, Koordinate = koordinate }));
    }

    [Authorize(Policy = "RequireTuristickiVodicRole")]
    [HttpPut("IzmeniKoorde/{id}")]
    public async Task<IActionResult> IzmeniKoorde(Guid id, [FromBody]Koordinata[] koordinate)
    {
        return HandleResult(await Mediator.Send(new IzmeniKoorde.Command { PodrucjeId = id, Koordinate = koordinate }));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("ObrisiKoorde/{id}")]
    public async Task<IActionResult> IzbrisiKoorde(Guid id)
    {
        return HandleResult(await Mediator.Send(new ObrisiKoorde.Command { PodrucjeId = id }));
    }
}
