using Application.Biljke;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BiljkaController : BaseApiController // potrebne izmene funkcija
{
    [AllowAnonymous]
    [HttpGet("VratiBiljke")]
    public async Task<IActionResult> VratiBiljke()
    {
        return HandleResult(await Mediator.Send(new List.Query()));
    }

    [AllowAnonymous]
    [HttpGet("VratiBiljku/{id}")]
    public async Task<IActionResult> VratiBiljku(Guid id)
    {
        return HandleResult(await Mediator.Send(new Details.Query{Id = id}));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpPost("KreirajBiljku")]
    public async Task<IActionResult> KreirajBiljku([FromForm]BiljkaDto biljka)
    {
        return HandleResult(await Mediator.Send(new Create.Command {Biljka = biljka}));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpPut("IzmeniBiljku/{id}")]
    public async Task<IActionResult> IzmeniBiljku(Guid id, Biljka biljka)
    {
        biljka.ID = id;
        return HandleResult(await Mediator.Send(new Edit.Command {Biljka = biljka}));
    }

    [Authorize(Policy = "RequireAdministratorRoleOrTuristickiVodicRole")]
    [HttpDelete("ObrisiBiljku/{id}")]
    public async Task<IActionResult> ObrisiBiljku(Guid id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command {Id = id}));
    }

    [AllowAnonymous]
    [HttpGet("VratiPodrucjaBiljke/{id}")]
    public async Task<IActionResult> VratiPodrucjaBiljke(Guid id)
    {
        return HandleResult(await Mediator.Send(new ListPodrucja.Query{BiljkaId = id}));
    }

    //TODO vrati sva podrucja biljke, sve biljke na podrucju
}
