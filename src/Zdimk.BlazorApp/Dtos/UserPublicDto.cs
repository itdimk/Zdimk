using System;

namespace Zdimk.BlazorApp.Dtos
{
    public class UserPublicDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public DateTime BirthDate { get; set; }
    }
}