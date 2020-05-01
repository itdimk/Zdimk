using System;

namespace Zdimk.BlazorApp.Dtos.Commands
{
    public class CreateUserCommand 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string Password { get; set; }
    }
}