using Microsoft.AspNetCore.Http;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Helpers
{
    public class TokenData : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string userId = "";
            try
            {
                ///Read Token And Remove Brefix"Bearer"
                var token = context.Request.Headers["Authorization"].ToString().Split(" ")[1];
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type.ToLower() == "id").Value;
            }
            catch
            {
                userId = null;
            }
            finally
            {
                LogContext.PushProperty("UserID", userId);
                await next(context);
            }

        }
    }
}
