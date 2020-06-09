using System;
using MediatR;

namespace Zdimk.Abstractions.Commands
{
    public class DeleteAlbumCommand : IRequest
    {
        public Guid AlbumId { get; set; }
    }
}