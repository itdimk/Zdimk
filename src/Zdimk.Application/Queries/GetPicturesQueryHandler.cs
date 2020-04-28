using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Zdimk.Application.Extensions;
using Zdimk.Application.Interfaces;
using Zdimk.DataAccess;
using Zdimk.Domain.Dtos;
using Zdimk.Domain.Entities;
using Zdimk.Domain.Extensions;

namespace Zdimk.Application.Queries
{
    public class GetPicturesQueryHandler : IRequestHandler<GetPicturesQuery, IEnumerable<PictureDto>>
    {
        private readonly ZdimkDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPictureService _pictureService;

        public GetPicturesQueryHandler(ZdimkDbContext dbContext,
            IHttpContextAccessor httpContextAccessor, IPictureService pictureService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _pictureService = pictureService;
        }

        public async Task<IEnumerable<PictureDto>> Handle(GetPicturesQuery request, CancellationToken cancellationToken)
        {
            Album album = await _dbContext.Albums.FindAsync(request.AlbumId);
            string userId = _httpContextAccessor.HttpContext.GetUserId();

            if (!album.IsPrivate || userId == album.OwnerId)
            {
                return album.Pictures.Select(p =>
                        p.ToPictureDto(_pictureService.GetPictureUrl(p.Id.ToString(), p.Extension)))
                    .ToArray();
            }
            else
                return new PictureDto[0];
        }
    }
}