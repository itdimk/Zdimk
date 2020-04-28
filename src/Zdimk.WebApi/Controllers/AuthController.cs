using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zdimk.Application.Commands;
using Zdimk.Application.Dtos;
using Zdimk.Application.Queries;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator, RoleManager<IdentityRole> m)
        {
            _mediator = mediator; // TODO: SHIT
        }
        
        [HttpPost]
        public async Task<ActionResult<JwtTokenPair>> GetJwtTokenPair(GetJwtTokenPairQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<string>> GetJwtAccessToken(GetJwtAccessTokenQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> RegisterJwtRefreshToken(RegisterJwtRefreshTokenCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}