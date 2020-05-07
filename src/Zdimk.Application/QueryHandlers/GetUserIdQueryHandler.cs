using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Exceptions;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.QueryHandlers
{
    public class GetUserIdQueryHandler : IRequestHandler<GetUserIdQuery, Guid>
    {
        private readonly UserManager<User> _userManager;


        public GetUserIdQueryHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Guid> Handle(GetUserIdQuery request, CancellationToken cancellationToken)
        {
            string userName = request.UserName;
            User user = await _userManager.FindByNameAsync(userName);

            if (user != null)
                return user.Id;
            else
                throw new RecordNotFoundException("User with this name not found");
        }
    }
}