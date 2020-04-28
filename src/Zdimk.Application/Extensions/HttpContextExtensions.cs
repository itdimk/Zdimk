using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Zdimk.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            Claim[] userClaims = context.User.Claims.ToArray();
            string id = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string altId =  userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (id != null) return id;
            if (altId != null) return altId;
            throw new Exception("Id is not found");
        }
    }
}