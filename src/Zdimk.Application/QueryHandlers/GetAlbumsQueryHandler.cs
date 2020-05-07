using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.QueryHandlers
{
    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MainDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public GetAlbumsQueryHandler(IHttpContextAccessor accessor, MainDbContext dbContext,
            UserManager<User> userManager)
        {
            _httpContextAccessor = accessor;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            Guid userId = _httpContextAccessor.HttpContext.GetUserId();
            User user = await _userManager.FindByIdAsync(userId.ToString());

            if (user.Id == request.UserId)
            {
                return await _dbContext.Albums.Skip(request.Offset).Take(request.Count)
                    .Select(a => a.ToAlbumDto())
                    .ToArrayAsync(cancellationToken);
            }
            else
            {
                return await _dbContext.Albums.Where(a => !a.IsPrivate).Skip(request.Offset).Take(request.Count)
                    .Select(a => a.ToAlbumDto())
                    .ToArrayAsync(cancellationToken);
            }
        }
    }
}