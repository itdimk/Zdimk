using System;

namespace Zdimk.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public  string Text { get; set; }

        public virtual Picture Picture { get; set; }
        public Guid PictureId { get; set; }
        
        public virtual  User Author { get; set; }
        public Guid AuthorId { get; set; }
    }
}