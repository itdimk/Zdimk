using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;

namespace Zdimk.Application.Commands
{
    public class DeactivateRefreshTokensCommandHandler : IRequestHandler<DeactivateRefreshTokensCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ZdimkDbContext _dbContext;

        public DeactivateRefreshTokensCommandHandler(IHttpContextAccessor httpContextAccessor, ZdimkDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeactivateRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            string userId = _httpContextAccessor.HttpContext.GetUserId();

            IdentityUserToken<string>[] tokensToDelete
                = await _dbContext.UserTokens.Where(t => t.UserId == userId).ToArrayAsync(cancellationToken);

            _dbContext.UserTokens.RemoveRange(tokensToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}