using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ecommerce.service;
using ecommerce.models;
using ecommerce.extention;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ecommerce.data;

namespace ecommerce.service
{
    public class TokenService : ITokenService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ISettings settings;

        public TokenService(ICustomerRepository authenticationRepo, ISettings settings)
        {
            this.customerRepository = authenticationRepo;
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
                    new Claim(ClaimTypes.Name, request.Email),
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