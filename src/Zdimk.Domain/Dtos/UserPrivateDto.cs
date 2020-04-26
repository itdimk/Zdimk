using System;

namespace Zdimk.Domain.Dtos
{
    public class UserPrivateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
    }
}