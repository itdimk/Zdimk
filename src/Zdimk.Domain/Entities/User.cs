using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace Zdimk.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
    }
}