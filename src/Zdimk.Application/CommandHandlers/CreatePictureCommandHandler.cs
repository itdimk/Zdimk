using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Extensions;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.CommandHandlers
{
    public class CreatePictureCommandHandler : IRequestHandler<CreatePictureCommand, PictureDto>
    {
        private readonly IPictureService _pictureService;
        private readonly MainDbContext _dbContext;

        public CreatePictureCommandHandler(IPictureService pictureService, MainDbContext dbContext)
        {
            _pictureService = pictureService;
            _dbContext = dbContext;
        }

        public async Task<PictureDto> Handle(CreatePictureCommand request, CancellationToken cancellationToken)
        {
            Picture picture = new Picture
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Extension = Path.GetExtension(request.PictureFile.FileName),
                Created = DateTimeOffset.UtcNow,
                AlbumId = request.AlbumId,
            };


            using (Stream source = request.PictureFile.OpenReadStream())
                await _pictureService.SaveToContentFolderAsync(source, picture.Id, picture.Extension);

            await _dbContext.Pictures.AddAsync(picture, cancellationToken);

            await SetDefaultAlbumCoverCoverIfRequiredAsync(picture.AlbumId, _pictureService.GetSmallPictureUrl(picture.Id,
                picture.Extension));
            await _dbContext.SaveChangesAsync(cancellationToken);
            return picture.ToPictureDto(_pictureService.GetBigPictureUrl(picture.Id, picture.Extension),
                _pictureService.GetSmallPictureUrl(picture.Id, picture.Extension));
        }

        private async Task SetDefaultAlbumCoverCoverIfRequiredAsync(Guid albumId, string coverUrl)
        {
            var album = await _dbContext.Albums.FindAsync(albumId);

            if (string.IsNullOrEmpty(album.CoverUrl))
                album.CoverUrl = coverUrl;
        }
    }
}