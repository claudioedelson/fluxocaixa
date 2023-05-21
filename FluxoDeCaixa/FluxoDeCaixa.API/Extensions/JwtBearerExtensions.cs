using Ardalis.GuardClauses;
using FluxoDeCaixa.Shared.AppSettings;
using FluxoDeCaixa.Shared.Constants;
using FluxoDeCaixa.Shared.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FluxoDeCaixa.API.Extensions
{
    internal static class JwtBearerExtensions
    {
        internal static void AddJwtBearer(
            this IServiceCollection services,
            IConfiguration configuration,
            bool isProduction)
        {
            Guard.Against.Null(configuration, nameof(configuration));

            var jwtOptions = configuration.GetOptions<JwtOptions>(AppSettingsKeys.JwtOptions);

            services
                .AddAuthentication(authOptions =>
                {
                    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    authOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearerOptions =>
                {
                    // RequireHttpsMetadata deve estar sempre habilitado no ambiente de produção.
                    bearerOptions.RequireHttpsMetadata = isProduction;
                    bearerOptions.SaveToken = true;
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Secret)),
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidIssuer = jwtOptions.Issuer
                    };
                });

            // Ativa o uso do token como forma de autorizar o acesso a recursos deste projeto.
            services.AddAuthorization(authOptions =>
            {
                authOptions.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });
        }
    }
}
