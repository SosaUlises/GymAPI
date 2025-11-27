using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sosa.Gym.Application.DataBase;
using Sosa.Gym.Application.External;
using Sosa.Gym.Domain.Entidades.Usuario;
using Sosa.Gym.External.GetTokenJWT;
using Sosa.Gym.Persistence.DataBase;
using System.Text;

namespace Sosa.Gym.External
{
    public static class InjeccionDependenciaService
    {
        public static IServiceCollection AddExternal(this IServiceCollection services,
        IConfiguration configuration)
        {
            
            // Conexion a PostgreSQL
           
            var connectionString = configuration.GetConnectionString("PostgreConnectionString");
            services.AddDbContext<DataBaseService>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IDataBaseService, DataBaseService>();


            // Identity
         
            services.AddIdentity<UsuarioEntity, IdentityRole<int>>(options =>
            {
                // Config password
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

            })
            .AddEntityFrameworkStores<DataBaseService>()
            .AddDefaultTokenProviders();

       
            // JWT – Configuración completa

            var jwtKey = configuration["Jwt_Key"];
            var jwtIssuer = configuration["Jwt_Issuer"];
            var jwtAudience = configuration["Jwt_Audience"];

            // VALIDACIÓN CRÍTICA: Si esto falla, es que no leyó el secreto
            if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
            {
                // Este Console.WriteLine te salvará la vida en los logs de Render
                Console.WriteLine($"[ERROR CRÍTICO] La Jwt_Key es nula o muy corta. Valor leído: '{jwtKey}'");

                // Si estamos en desarrollo, lanzamos error para que te des cuenta
                // if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                //    throw new Exception("JWT Key no configurada en Secrets o AppSettings.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });


            // Servicios JWT

            services.AddScoped<IGetTokenJWTService, GetTokenJWTService>();

            return services;
        }
    }
}
