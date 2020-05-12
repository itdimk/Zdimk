using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.CommandHandlers;
using Zdimk.Application.QueryHandlers;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator; 
        }
        
        [HttpPost]
        public async Task<ActionResult<JwtTokenPair>> GetJwtTokenPair(GetTokenPairQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<string>> GetJwtAccessToken(GetAccessTokenQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult> RegisterJwtRefreshToken(ActivateRefreshTokenCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}