using Application.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class SlikeController : BaseApiController
{
    [HttpPost("DodajSliku")]
    public async Task<IActionResult> Add([FromForm] Add.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpDelete("ObrisiSliku/{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
    }

    [HttpPost("{id}/setMain")]
    public async Task<IActionResult> SetMain(string id)
    {
        return HandleResult(await Mediator.Send(new SetMain.Command { Id = id }));
    }
}