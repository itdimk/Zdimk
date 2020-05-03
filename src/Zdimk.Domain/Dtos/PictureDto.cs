using System;
using Zdimk.Domain.Entities;

namespace Zdimk.Domain.Dtos
{
    public class PictureDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        
        public Guid AlbumId { get; set; }
        public Guid OwnerId { get; set; }
        
        public int Likes { get; set; }
    }
}