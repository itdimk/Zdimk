using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Abstractions.Commands;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;

namespace Zdimk.Application.CommandHandlers
{
    public class DeactivateRefreshTokensCommandHandler : IRequestHandler<FullLogoutCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MainDbContext _dbContext;

        public DeactivateRefreshTokensCommandHandler(IHttpContextAccessor httpContextAccessor, MainDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(FullLogoutCommand request, CancellationToken cancellationToken)
        {
            Guid userId = _httpContextAccessor.HttpContext.GetUserId();

            IdentityUserToken<Guid>[] tokensToDelete
                = await _dbContext.UserTokens.Where(t => t.UserId == userId).ToArrayAsync(cancellationToken);

            _dbContext.UserTokens.RemoveRange(tokensToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}