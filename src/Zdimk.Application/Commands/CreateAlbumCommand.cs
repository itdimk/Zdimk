using MediatR;
using Zdimk.Domain.Dtos;

namespace Zdimk.Application.Commands
{
    public class CreateAlbumCommand : IRequest<AlbumDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
    }
}