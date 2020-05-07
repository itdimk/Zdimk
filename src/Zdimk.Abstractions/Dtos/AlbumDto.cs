using System;

namespace Zdimk.Abstractions.Dtos
{
    public class AlbumDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Updated { get; set; }
        public DateTimeOffset Created { get; set; }
        public string CoverUrl { get; set; }
        public bool IsPrivate { get; set; }
        public Guid OwnerId { get; set; }
    }
}