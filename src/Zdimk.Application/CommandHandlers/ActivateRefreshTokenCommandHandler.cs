using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Abstractions.Commands;
using Zdimk.Application.Constants;
using Zdimk.Application.Extensions;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.CommandHandlers
{
    public class ActivateRefreshTokenCommandHandler : IRequestHandler<ActivateRefreshTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly MainDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActivateRefreshTokenCommandHandler(UserManager<User> userManager, MainDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(ActivateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Guid userId = _httpContextAccessor.HttpContext.GetUserId();
            User user = await _userManager.FindByIdAsync(userId.ToString());

            bool isValidToken = await _userManager.VerifyUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Refresh,
                request.RefreshToken);

            var tokens = await _dbContext.UserTokens.Where(t => t.UserId == userId).ToArrayAsync(cancellationToken);

            if (tokens.Length > 5)
                _dbContext.UserTokens.RemoveRange(tokens);

            if (isValidToken)
            {
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Thumbprint, request.Thumbprint));
                await _dbContext.UserTokens.AddAsync(new IdentityUserToken<Guid>
                {
                    LoginProvider = "Zdimk",
                    Name = DateTime.Now.Ticks.ToString(),
                    UserId = user.Id,
                    Value = request.RefreshToken
                }, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
                throw new ArgumentException("Invalid token");

            return Unit.Value;
        }
    }
}