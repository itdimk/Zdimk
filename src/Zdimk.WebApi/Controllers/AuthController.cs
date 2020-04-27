using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    }
}