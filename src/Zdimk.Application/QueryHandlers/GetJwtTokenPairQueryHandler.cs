﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;
using Zdimk.Application.Constants;
using Zdimk.Application.Exceptions;
using Zdimk.Domain.Entities;

namespace Zdimk.Application.QueryHandlers
{
    public class GetJwtTokenPairQueryHandler : IRequestHandler<GetTokenPairQuery, JwtTokenPair>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public GetJwtTokenPairQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<JwtTokenPair> Handle(GetTokenPairQuery request, CancellationToken cancellationToken)
        {
            User user;
            if (request.Login.Contains("@"))
                user = await _userManager.FindByEmailAsync(request.Login);
            else
                user = await _userManager.FindByNameAsync(request.Login);

            if (user != null)
            {
                SignInResult signInResult =
                    await _signInManager.PasswordSignInAsync(user, request.Password, false, false);

                if (signInResult == SignInResult.Success)
                {
                    string accessToken =
                        await _userManager.GenerateUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Access);
                    string refreshToken =
                        await _userManager.GenerateUserTokenAsync(user, "jwt", JwtSecurityTokenPurposes.Refresh);

                    return new JwtTokenPair {AccessToken = accessToken, RefreshToken = refreshToken};
                }
            }

            throw new RecordNotFoundException("Incorrect password or login");
        }
    }
}