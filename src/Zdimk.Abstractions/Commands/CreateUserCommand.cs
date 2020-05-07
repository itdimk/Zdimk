using System;
using MediatR;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.Abstractions.Commands
{
    public class CreateUserCommand : IRequest<UserPrivateDto>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
    }
}