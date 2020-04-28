using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;
using Zdimk.Domain.Extensions;

namespace Zdimk.Application.Queries
{
    public class GetAlbumsQueryHandler : IRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ZdimkDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public GetAlbumsQueryHandler(IHttpContextAccessor accessor, ZdimkDbContext dbContext,
            UserManager<User> userManager)
        {
            _httpContextAccessor = accessor;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<AlbumDto>> Handle(GetAlbumsQuery request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByIdAsync(_httpContextAccessor.HttpContext.GetUserId());

            if (user.UserName == request.UserName)
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