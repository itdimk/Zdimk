using System;
using MediatR;

namespace Zdimk.Abstractions.Queries
{
    public class GetUserIdQuery : IRequest<Guid>
    {
        public string UserName { get; set; }
    }
}