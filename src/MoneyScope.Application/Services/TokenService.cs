using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyScope.Application.Interfaces;
using MoneyScope.Application.Models.Token;
using MoneyScope.Core.Models;
using MoneyScope.Core.Token;
using MoneyScope.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MoneyScope.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly TokenConfigurations _tokenConfig;
        private readonly IConfiguration _config;

        public TokenService(TokenConfigurations tokenConfig, IConfiguration config)
        {
            _tokenConfig = tokenConfig;
            _config = config;
        }
        public async Task<ResponseModel<dynamic>> GenerateTokenByEmail(string email)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("functionality", "Authorized"));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, email));

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(email, "Login"), claims);

            identity.Claims.ToList();

            DateTime dtCreation = DateTime.Now;
            DateTime dtExpiration;
            dtExpiration = dtCreation +
            TimeSpan.FromSeconds(1800);

            var key = Encoding.ASCII.GetBytes(_config["TokenConfigurations:Key"]!);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _config["TokenConfigurations:Issuer"],
                Audience = _config["TokenConfigurations:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = dtCreation,
                Expires = dtExpiration
            });

            var token = handler.WriteToken(securityToken);

            return await Task.FromResult(FactoryResponse<dynamic>.Success(token));
        }
        public async Task<ResponseModel<dynamic>> GerenerateToken(GenerateTokenModel model)
        {
            var claims = new List<Claim>();

            foreach (var f in model.Modules)
            {
                claims.Add(new Claim("functionality", f.Trim()));
            }

            var uniqueName = "";
            uniqueName = model.UserEmail;

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")));
            claims.Add(new Claim(JwtRegisteredClaimNames.UniqueName, uniqueName));
            claims.Add(new Claim("id", model.UserId.ToString()));

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(uniqueName, "Login"), claims);

            identity.Claims.ToList();

            DateTime dtCreation = DateTime.UtcNow;
            DateTime dtExpiration;
            if (model.Seconds != null && model.Seconds > 0)
            {
                dtExpiration = dtCreation +
                TimeSpan.FromSeconds(model.Seconds.Value);
            }
            else if (_tokenConfig.Seconds != 0)
            {
                dtExpiration = dtCreation +
                TimeSpan.FromSeconds(_tokenConfig.Seconds);
            }
            else
            {
                dtExpiration = dtCreation +
                TimeSpan.FromSeconds(604800);
            }

            var key = Encoding.ASCII.GetBytes(_config["TokenConfigurations:Key"]!);
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _config["TokenConfigurations:Issuer"],
                Audience = _config["TokenConfigurations:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = dtCreation,
                Expires = dtExpiration,
            });

            var token = handler.WriteToken(securityToken);

            var unvalidatedToken = handler.ReadJwtToken(token);

            return await Task.FromResult(FactoryResponse<dynamic>.Success(new
            {
                model.UserId,
                model.UserName,
                model.UserEmail,
                model.ModulesAssembled,
                model.UserProfileImage,
                dtCreation,
                dtExpiration,
                Token = token
            }));
        }
        public string? Getclaim(string claim, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["TokenConfigurations:Key"]!);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["TokenConfigurations:Issuer"],
                    ValidAudience = _config["TokenConfigurations:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                IEnumerable<Claim> claims = jwtToken.Claims;

                var result = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?.Value;
                return result;
            }
            catch
            {
                return null;
            }
        }
        public bool IsValidToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["TokenConfigurations:Key"]!);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["TokenConfigurations:Issuer"],
                    ValidAudience = _config["TokenConfigurations:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                DateTime validTo = jwtToken.ValidTo;

                if (DateTime.Now > validTo) return false;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
