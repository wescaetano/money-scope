using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MoneyScope.Api.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class AuthorizationExtension
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationConfiguration(this IServiceCollection services)
        {
            services.AddAuthorization(opt =>
            {
                opt.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build();

                List<string> funcionalidades =
                [
                    "Users",
                    "Access Profiles"
                ];
                foreach (var f in funcionalidades)
                {
                    AddAccessByClaimsFuncionality(opt, $"{f}-C");
                    AddAccessByClaimsFuncionality(opt, $"{f}-E");
                    AddAccessByClaimsFuncionality(opt, $"{f}-V");
                    AddAccessByClaimsFuncionality(opt, $"{f}-I");
                    AddAccessByClaimsFuncionality(opt, $"{f}-EX");
                }
            });
        }

        private static void AddAccessByClaimsFuncionality(AuthorizationOptions options, string type)
        {
            options.AddPolicy($"{type}", a => AddAccessByClaims(a, [new Claim("modulo", type)]));
        }

        private static void AddAccessByClaims(AuthorizationPolicyBuilder builder, List<Claim> claims)
        {
            builder.RequireAssertion(c =>
            {
                try
                {
                    return ValidateMultiClaims(c.User, claims);
                }
                catch (Exception e)
                {
                    string msg = e.Message;
                    return false;
                }
            });
        }

        private static bool ValidateMultiClaims(ClaimsPrincipal principal, List<Claim> claims)
        {
            bool allow = false;
            foreach (var claim in claims)
            {
                bool exist = principal.Claims.ToList().Exists(c => c.Value == claim.Value);
                if (exist) allow = true;
            }
            return allow;
        }
    }
}
