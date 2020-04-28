using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;
using Zdimk.Domain.Extensions;

namespace Zdimk.Application.Commands
{
    public class CreatePictureCommandHandler : IRequestHandler<CreatePictureCommand, PictureDto>
    {
        private readonly IPictureService _pictureService;
        private readonly ZdimkDbContext _dbContext;

        public CreatePictureCommandHandler(IPictureService pictureService, ZdimkDbContext dbContext)
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
                Extension = Path.GetExtension(request.PictureFile.Name),
                Created = DateTimeOffset.UtcNow,
                AlbumId = request.AlbumId
            };
            
            using (Stream source = request.PictureFile.OpenReadStream())
                await _pictureService.SaveToContentFolderAsync(source, picture.Id.ToString(), picture.Extension);

            await _dbContext.Pictures.AddAsync(picture, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return picture.ToPictureDto(_pictureService.GetPictureUrl(picture.Id.ToString(), picture.Extension));
        }
    }
}