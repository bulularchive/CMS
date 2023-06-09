using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Interfaces;
using CoreLayer.Entities;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Api.Utils
{
    public class JwtHandler : IJwtHandler
    {
        private readonly IConfiguration _configuration;
        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(Account account)
        {
            var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["JwtSetting:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(ClaimTypes.Role, Enum.GetName(account.Role) ?? string.Empty),
            new Claim("Id", account.AccountId.ToString()),
            new Claim("Name", account.Email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:SecretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_configuration["JwtSetting:ValidIssuer"], _configuration["JwtSetting:ValidAudience"], claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSetting:LifeTime"])),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetAccountIdFromJwt(StringValues headerValue)
        {
            var token = headerValue.ToString();
            var tokens = token.Split(" ");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(tokens.Count() == 1 ? tokens[0] : tokens[1]);
            var accountId = jwtSecurityToken.Claims.First(claim => claim.Type == "Id").Value;
            return accountId;
        }
    }
}