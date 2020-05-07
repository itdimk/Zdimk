using Zdimk.Abstractions.Dtos;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Extensions
{
    public static class DtoConverters
    {
        public static UserPrivateDto ToUserPrivateDto(this User user)
        {
            return new UserPrivateDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                BirthDate = user.BirthDate,
                RegistrationDate = user.RegistrationDate,
                LastLoginDate = user.LastLoginDate,
                UserName = user.UserName
            };
        }

        public static UserPublicDto ToUserPublicDto(this User user)
        {
            return new UserPublicDto
            {
                Id = user.Id,
                UserName = user.FullName,
                BirthDate = user.BirthDate,
                FullName = user.FullName
            };
        }

        public static AlbumDto ToAlbumDto(this Album album)
        {
            return new AlbumDto
            {
                Id = album.Id,
                Name = album.Name,
                Description = album.Description,
                CoverUrl = album.CoverUrl,
                Updated = album.Updated,
                Created = album.Created,
                IsPrivate = album.IsPrivate,
                OwnerId = album.OwnerId
            };
        }

        public static PictureDto ToPictureDto(this Picture picture, string url)
        {
            return new PictureDto
            {
                Id = picture.Id,
                Name = picture.Name,
                Url = url,
                Description = picture.Description,
                Created = picture.Created,
                AlbumId = picture.AlbumId,
            };
        }
    }
}