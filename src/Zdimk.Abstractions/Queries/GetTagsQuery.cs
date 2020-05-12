using System.Collections;
using System.Collections.Generic;
using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Queries
{
    public class GetTagsQuery : IRequest<IEnumerable<TagDto>>
    {
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}