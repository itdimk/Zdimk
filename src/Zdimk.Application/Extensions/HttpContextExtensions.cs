using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Zdimk.Application.Extensions
{
    public static class HttpContextExtensions
    {
        public static Guid GetUserId(this HttpContext context)
        {
            string id = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string altId = context.User.FindFirstValue(ClaimTypes.Name);

            if (Guid.TryParse(id, out Guid result))
                return result;

            if (Guid.TryParse(altId, out Guid altResult))
                return altResult;

            throw new Exception("Id is not found");
        }
    }
}