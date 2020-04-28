using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Application.Constants;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Commands
{
    public class RegisterRefreshTokenCommandHandler : IRequestHandler<RegisterJwtRefreshTokenCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ZdimkDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContext;

        public RegisterRefreshTokenCommandHandler(UserManager<User> userManager, ZdimkDbContext dbContext,
            IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _httpContext = httpContext;
        }

        public async Task<Unit> Handle(RegisterJwtRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            string userId = _httpContext.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userManager.FindByIdAsync(userId);
            
            bool isValidToken = await _userManager.VerifyUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Refresh,
                request.RefreshToken);

            var  tokens = await _dbContext.UserTokens.Where(t => t.UserId == userId).ToArrayAsync(cancellationToken);

            if(tokens.Length > 5)
                _dbContext.UserTokens.RemoveRange(tokens);
            
            if (isValidToken)
            {
                await _dbContext.UserTokens.AddAsync(new IdentityUserToken<string>
                {
                    LoginProvider = "Zdimk",
                    Name = "jwt-refresh-token",
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