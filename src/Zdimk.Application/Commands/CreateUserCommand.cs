using System;
using MediatR;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Commands
{
    public class CreateUserCommand : IRequest<UserPrivateDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
    }
}