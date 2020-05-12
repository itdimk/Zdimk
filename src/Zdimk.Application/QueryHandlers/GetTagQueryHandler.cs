using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;

namespace Zdimk.Application.QueryHandlers
{
    public class GetTagQueryHandler : IRequestHandler<GetTagsQuery, IEnumerable<TagDto>>
    {
        private readonly MainDbContext _dbContext;

        public GetTagQueryHandler(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.PictureTags.Skip(request.Offset).Take(request.Count)
                .Select(t => t.ToTagDto())
                .ToArrayAsync(cancellationToken);
        }
    }
}