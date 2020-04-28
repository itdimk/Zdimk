using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zdimk.Application.Constants;
using Zdimk.DataAccess;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.Queries
{
    public class GetJwtAccessTokenQueryHandler : IRequestHandler<GetJwtAccessTokenQuery, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly ZdimkDbContext _dbContext;

        public GetJwtAccessTokenQueryHandler(UserManager<User> userManager, ZdimkDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<string> Handle(GetJwtAccessTokenQuery request, CancellationToken cancellationToken)
        {
            var token = await _dbContext.UserTokens.FirstOrDefaultAsync(t =>
                t.Value == request.JwtRefreshToken, cancellationToken);

            if (token == null) 
                throw new ArgumentException("Invalid token");

            User user = await _userManager.FindByIdAsync(token.UserId);

            var thumbprintClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c =>
                c.Type == ClaimTypes.Thumbprint && c.Value == request.Thumbprint);

            if(thumbprintClaim == null) 
                throw new ArgumentException("You are fucking hacker");
            
            return await _userManager.GenerateUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Access);
        }
    }
}