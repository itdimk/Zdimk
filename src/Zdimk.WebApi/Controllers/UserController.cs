using System;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<UserPrivateDto>> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpGet]
        public async Task<ActionResult<Guid>> GetUserId(GetUserIdQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}