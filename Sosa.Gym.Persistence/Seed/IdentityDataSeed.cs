using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sosa.Gym.Domain.Entidades.Usuario;

namespace Sosa.Gym.Persistence.Seed
{
    public class IdentityDataSeed
    {
        public static async Task SeedRolesAsync(IHost app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UsuarioEntity>>();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            // Roles
            string[] roles = { "Cliente", "Administrador" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            // Crear Administrador
            var adminEmail = config["Admin_Email"];
            var adminPassword = config["Admin_Password"];

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
                throw new Exception("Debes configurar Admin:Email y Admin:Password como secreto");

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new UsuarioEntity
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    Dni = 000000
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);

                if (!createResult.Succeeded)
                {
                    throw new Exception("No se pudo crear el usuario administrador: " +
                                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(adminUser, "Administrador");
            }
        }
    }
}
