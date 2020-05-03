using System;
using System.Collections;
using System.Collections.Generic;
using MediatR;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Queries
{
    public class GetAlbumsQuery : IRequest<IEnumerable<AlbumDto>>
    {
        public Guid UserId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}