using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FruitSA.Web.Helper
{
    public static class JwtHelper
    {
        public static string GetUsernameFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email || c.Type == "email");
                return emailClaim?.Value;
            }
            catch (Exception)
            {
                // You could log the exception here if needed
                return null;
            }
        }
    }
}
