using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Commands
{
    public class CreateAlbumCommand : IRequest<AlbumDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string CoverUrl { get; set; }
        public bool IsPrivate { get; set; }
    }
}