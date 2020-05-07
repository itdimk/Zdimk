using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Zdimk.Abstractions.Commands;
using Zdimk.Abstractions.Dtos;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.CommandHandlers
{
    public class CreateAlbumCommandHandler : IRequestHandler<CreateAlbumCommand, AlbumDto>
    {
        private readonly MainDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateAlbumCommandHandler(MainDbContext dbContext, UserManager<User> userManager,
            IHttpContextAccessor accessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _httpContextAccessor = accessor;
        }

        public async Task<AlbumDto> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _httpContextAccessor.HttpContext.GetUserId();

            var album = new Album
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Updated = DateTimeOffset.UtcNow,
                Created = DateTimeOffset.UtcNow,
                IsPrivate = request.IsPrivate,
                OwnerId = userId,
                CoverUrl = request.CoverUrl,
            };

            await _dbContext.Albums.AddAsync(album, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return album.ToAlbumDto();
        }
    }
}