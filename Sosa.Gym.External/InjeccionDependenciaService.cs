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

            var jwtKey = configuration["Jwt:Key"];
            var jwtIssuer = configuration["Jwt:Issuer"];
            var jwtAudience = configuration["Jwt:Audience"];

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
