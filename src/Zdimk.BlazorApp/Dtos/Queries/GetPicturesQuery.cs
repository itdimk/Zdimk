using System;
using System.Collections.Generic;

namespace Zdimk.BlazorApp.Dtos.Queries
{
    public class GetPicturesQuery 
    {
        public Guid AlbumId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}