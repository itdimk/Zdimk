using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Zdimk.Domain.Dtos;

namespace Zdimk.Application.Commands
{
    public class CreatePictureCommand : IRequest<PictureDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid AlbumId { get; set; }
        public IFormFile PictureFile { get; set; }
    }
}