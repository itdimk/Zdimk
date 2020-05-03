using System;
using System.Collections;
using System.Collections.Generic;

namespace Zdimk.Domain.Entities
{
    public class Picture
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Extension { get; set; }
        public DateTimeOffset Created { get; set; }
        public virtual  Album Album { get; set; }
        
        public virtual  ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PictureTag> PictureTags { get; set; }
        public Guid AlbumId { get; set; }
        
        public int Likes { get; set; }
    }
}