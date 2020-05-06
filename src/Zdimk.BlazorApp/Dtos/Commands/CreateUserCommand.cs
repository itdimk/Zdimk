using System;

namespace Zdimk.BlazorApp.Dtos.Commands
{
    public class CreateUserCommand 
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Password { get; set; }
    }
}