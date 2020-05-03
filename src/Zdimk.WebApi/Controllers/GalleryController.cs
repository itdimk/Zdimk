using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zdimk.Application.Commands;
using Zdimk.Application.Queries;
using Zdimk.Domain.Dtos;

namespace Zdimk.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class GalleryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GalleryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<PictureDto>> CreatePicture([FromForm] CreatePictureCommand command)
            => await _mediator.Send(command);

        [HttpPost]
        public async Task<ActionResult<AlbumDto>> CreateAlbum(CreateAlbumCommand command)
            => Ok(await _mediator.Send(command));

        [HttpPost]
        public async Task<ActionResult<IEnumerable<AlbumDto>>> GetAlbums(GetAlbumsQuery query)
            => Ok(await _mediator.Send(query));

        [HttpPost]
        public async Task<ActionResult<IEnumerable<PictureDto>>> GetPictures(GetPicturesQuery query)
            => Ok(await _mediator.Send(query));
    }
}