using System;
using BlazorInputFile;
using Microsoft.AspNetCore.Http;

namespace Zdimk.BlazorApp.Dtos.Commands
{
    public class CreatePictureCommand 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string AlbumId { get; set; }
        public IFileListEntry PictureFile { get; set; }
    }
}