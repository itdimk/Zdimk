using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Extensions;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.QueryHandlers
{
    public class GetPicturesQueryHandler : IRequestHandler<GetPicturesQuery, IEnumerable<PictureDto>>
    {
        private readonly MainDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPictureService _pictureService;

        public GetPicturesQueryHandler(MainDbContext dbContext,
            IHttpContextAccessor httpContextAccessor, IPictureService pictureService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _pictureService = pictureService;
        }

        public async Task<IEnumerable<PictureDto>> Handle(GetPicturesQuery request, CancellationToken cancellationToken)
        {
            Album album = await _dbContext.Albums.FindAsync(request.AlbumId);
            Guid userId = _httpContextAccessor.HttpContext.GetUserId();

            if (!album.IsPrivate || userId == album.OwnerId)
            {
                return album.Pictures.Select(p =>
                        p.ToPictureDto(_pictureService.GetPictureUrl(p.Id, p.Extension)))
                    .ToArray();
            }
            else
                return new PictureDto[0];
        }
    }
}