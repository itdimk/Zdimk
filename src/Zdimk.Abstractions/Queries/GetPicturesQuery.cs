using System;
using System.Collections.Generic;
using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Queries
{
    public class GetPicturesQuery : IRequest<IEnumerable<PictureDto>>
    {
        public Guid AlbumId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}