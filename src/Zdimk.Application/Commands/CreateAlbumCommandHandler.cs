using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;
using Zdimk.Domain.Extensions;

namespace Zdimk.Application.Commands
{
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, AlbumDto>
    {
        private readonly ZdimkDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateAlbumCommandHandler(ZdimkDbContext dbContext, UserManager<User> userManager,
            IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = accessor;
        }

        public async Task<AlbumDto> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            string userId = _httpContextAccessor.HttpContext.GetUserId();

            var album = new Album
            {
                Name = request.Name,
                Description = request.Description,
                Updated = DateTimeOffset.UtcNow,
                Created = DateTimeOffset.UtcNow,
                IsPrivate = request.IsPrivate,
                OwnerId = userId,
            };

            await _dbContext.Albums.AddAsync(album, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return album.ToAlbumDto();
        }
    }
}