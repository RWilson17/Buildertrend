using BuildertrendMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuildertrendMVC.Services
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            // Crear rol Admin si no existe
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Admin" });
            }

            // Crear o actualizar usuario Admin por defecto
            var adminEmail = "admin@buildertrend.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Crear nuevo usuario admin
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Currency = "USD",
                    Language = "es"
                };

                // Contraseña por defecto
                var result = await userManager.CreateAsync(admin, "Admin123!@");

                if (result.Succeeded)
                {
                    // Asignar rol Admin
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
            else
            {
                // Usuario existe, verificar que tenga rol Admin
                var adminRoles = await userManager.GetRolesAsync(adminUser);
                if (!adminRoles.Contains("Admin"))
                {
                    // Asignar rol Admin si no lo tiene
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

