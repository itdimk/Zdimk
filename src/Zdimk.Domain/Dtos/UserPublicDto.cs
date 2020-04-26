using System;

namespace Zdimk.Domain.Dtos
{
    public class UserPublicDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset BirthDate { get; set; }
    }
}