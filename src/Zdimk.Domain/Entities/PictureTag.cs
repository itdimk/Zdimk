using System;

namespace Zdimk.Domain.Entities
{
    public class PictureTag
    {
        public Guid PictureId { get; set; }
        public virtual Picture Picture { get; set; }
        public string TagName { get; set; }
        public virtual Tag Tag { get; set; }
    }
}