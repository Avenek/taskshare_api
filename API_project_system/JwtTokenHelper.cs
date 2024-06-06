using API_project_system;
using Microsoft.IdentityModel.Tokens;
using API_project_system.Entities;
using API_project_system.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_project_system
{
    public class JwtTokenHelper
    {
        private readonly AuthenticationSettings authenticationSettings;
        public JwtTokenHelper()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
           .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
            authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
        }

        public JwtSecurityToken ReadToken(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            return token;
        }


        public string CreateJwtToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim("Status", user.StatusId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer, authenticationSettings.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public bool IsTokenValid(string jwtToken)
        {
            var token = ReadToken(jwtToken);
            return token.ValidTo > DateTime.UtcNow;
        }
    }
}
