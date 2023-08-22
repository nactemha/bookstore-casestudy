using System.Text;
using ecommerce.models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ecommerce.data;

namespace ecommerce.service
{
    public class TokenService : ITokenService
    {
        private readonly ISettings settings;

        public TokenService(ISettings settings)
        {
            this.settings = settings;
        }
        public TokenInfo GetToken(LoginRequest request)
        {
            // Creates the signed JWT
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.GetSecretKey()));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, request.Email),
                }),
                Expires = DateTime.UtcNow.Add(settings.GetTokenExperation()),
                //Issuer = "MyWebsite.com",
                //Audience = "MyWebsite.com",
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(token);

            return new TokenInfo
            {
                Token = accessToken,
            };
        }
    }

}