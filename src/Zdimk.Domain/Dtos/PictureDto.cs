using System;
using Zdimk.Domain.Entities;

namespace Zdimk.Domain.Dtos
{
    public class PictureDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}