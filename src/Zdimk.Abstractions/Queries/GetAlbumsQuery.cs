using System;
using System.Collections.Generic;
using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Queries
{
    public class GetAlbumsQuery : IRequest<IEnumerable<AlbumDto>>
    {
        public Guid UserId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}