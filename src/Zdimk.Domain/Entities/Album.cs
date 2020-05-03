using System;
using System.Collections;
using System.Collections.Generic;

namespace Zdimk.Domain.Entities
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset Created { get; set; }
        public string CoverUrl { get; set; }
        public bool IsPrivate { get; set; }
        public virtual User Owner { get; set; }
        public virtual ICollection<Picture> Pictures { get; set; }
        public Guid OwnerId { get; set; }
        
    }
}