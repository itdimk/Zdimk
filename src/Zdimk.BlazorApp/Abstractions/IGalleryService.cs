﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zdimk.BlazorApp.Dtos;
using Zdimk.BlazorApp.Dtos.Queries;

namespace Zdimk.BlazorApp.Abstractions
{
    public interface IGalleryService
    {
        Task<ICollection<AlbumDto>> GetAlbumsAsync(GetAlbumsQuery query);
        Task<ICollection<PictureDto>> GetPicturesAsync(GetPicturesQuery query);
    }
}