using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Extensions;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.CommandHandlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserPrivateDto>
    {
        private readonly UserManager<User> _userManager;

        public CreateUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserPrivateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                FullName = request.FullName,
                RegistrationDate = DateTimeOffset.UtcNow,
                LastLoginDate = DateTimeOffset.UtcNow,
                BirthDate = request.BirthDate,
                Email = request.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, request.Password);

            if (result == IdentityResult.Success)
                return user.ToUserPrivateDto();
            else
                throw new Exception("Can't create user"); // TODO: add error message
        }
    }
}