using System;

namespace Zdimk.Abstractions.Dtos
{
    public class UserPrivateDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
    }
}