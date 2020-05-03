using System;
using MediatR;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Commands
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