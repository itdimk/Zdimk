using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace Zdimk.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }
        public DateTime BirthDate { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}