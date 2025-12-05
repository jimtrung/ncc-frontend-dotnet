using System;
using System.IdentityModel.Tokens.Jwt;

namespace Theater_Management_FE.Utils
{
    public static class JwtUtil
    {
        public static Guid? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => 
                c.Type == "sub" || c.Type == "id");

            if (userIdClaim == null)
                return null;

            return Guid.Parse(userIdClaim.Value);
        }
    }
}
