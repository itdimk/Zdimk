using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
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
            // TODO: try optimize this
            var token = await _dbContext.UserTokens.FirstOrDefaultAsync(t =>
                t.Value == request.JwtRefreshToken, cancellationToken);

            if (token == null) throw new ArgumentException("Invalid token");

            User user = await _userManager.FindByIdAsync(token.UserId); // TODO: add token validation
            return await _userManager.GenerateUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Access);
        }
    }
}