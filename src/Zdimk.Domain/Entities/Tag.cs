using System.Collections;
using System.Collections.Generic;

namespace Zdimk.Domain.Entities
{
    public class Tag
    {
        public string TagName { get; set; }
        public virtual ICollection<PictureTag> PictureTags { get; set; }
    }
}