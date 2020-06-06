using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;

namespace Zdimk.BlazorApp.Pages
{
    public partial class UploadPicturesPage
    {
        [Parameter] public string AlbumId { get; set; }

        private bool IsFirstImageUploading { get; set; } = true;

        private string ShowBigUploadLinkCss => IsFirstImageUploading ? "" : "collapsed";
        
        private List<PictureDto> Pictures { get; set; } = new List<PictureDto>();


        private Guid AlbumGuid
        {
            get
            {
                if (Guid.TryParse(AlbumId, out Guid guid))
                    return guid;
                return default;
            }
        }

        private async Task OnUploadedFilesChanged(IFileListEntry[] arg)
        {
           
            foreach (IFileListEntry file in arg)
            {
                IsFirstImageUploading = false;
                CreatePictureCommand command = await CreateCommand(file);
                Pictures.Add(await Mediator.Send(command));
                StateHasChanged();
            }
        }

        private async Task<CreatePictureCommand> CreateCommand(IFileListEntry file)
        {
            MemoryStream fileStream = await file.ReadAllAsync();

            var pictureFile = new FormFile(fileStream, 0, file.Size, file.Name, file.Name);

            return new CreatePictureCommand
            {
                Name = "",
                Description = "",
                AlbumId = AlbumGuid,
                PictureFile = pictureFile
            };
        }
    }
}