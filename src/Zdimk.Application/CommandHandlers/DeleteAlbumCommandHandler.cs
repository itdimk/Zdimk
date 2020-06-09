using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Extensions;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.CommandHandlers
{
    public class DeleteAlbumCommandHandler : IRequestHandler<DeleteAlbumCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MainDbContext _dbContext;
        private readonly IPictureService _pictureService;

        public DeleteAlbumCommandHandler(IHttpContextAccessor httpContextAccessor, MainDbContext dbContext, IPictureService pictureService)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
            _pictureService = pictureService;
        }

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            Guid userId = _httpContextAccessor.HttpContext.GetUserId();
            Album albumToDelete = await _dbContext.Albums.FindAsync(request.AlbumId);

            if (albumToDelete.OwnerId == userId)
            {
                foreach (var picture in albumToDelete.Pictures)
                    _pictureService.DeletePicture(picture.Id, picture.Extension);

                _dbContext.Pictures.RemoveRange(albumToDelete.Pictures);
                _dbContext.Albums.Remove(albumToDelete);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}