using Application.Komentari;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[AllowAnonymous]

public class KomentariController : BaseApiController
{
    private readonly IMediator _mediator;

    public KomentariController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("ObrisiSveKomentareObilaska{obilazakId}")]
    public async Task<IActionResult> DeleteAllComments(Guid obilazakId)
    {
        return HandleResult(await _mediator.Send(new DeleteAll.Command { ObilazakId = obilazakId }));
    }
}