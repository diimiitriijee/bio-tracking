using Application.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize]

public class ProfilesController : BaseApiController
    {
        [HttpGet("VratiProfil/{username}")]
        public async Task<IActionResult> VratiProfil(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }

        [HttpPut("IzmeniProfil")]
        public async Task<IActionResult> IzmeniProfil(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpGet("VratiObilaskeKorisnika{username}/obilasci")]
        public async Task<IActionResult> VratiObilaskeKorisnika(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListObilasci.Query{ 
                Username = username, Predicate = predicate 
                }));
        }

    }