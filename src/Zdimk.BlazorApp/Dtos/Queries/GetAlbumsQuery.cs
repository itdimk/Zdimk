using System;
using System.Collections.Generic;

namespace Zdimk.BlazorApp.Dtos.Queries
{
    public class GetAlbumsQuery 
    {
        public Guid UserId { get; set; }
        public int Offset { get; set; }
        public int Count { get; set; }
    }
}