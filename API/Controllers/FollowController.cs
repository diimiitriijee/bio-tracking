using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Pratioci;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FollowController : BaseApiController
    {
        [HttpPost("ZapratiOtprati/{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command
            { TargetUsername = username }));
        }

        [HttpGet("VratiPracenja/{username}")]//ali ako navede da oce da vrati ko njega prati i to moze
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query { Username = username, Predicate = predicate }));
        }
    }
}