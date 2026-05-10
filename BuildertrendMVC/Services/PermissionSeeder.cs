using BuildertrendMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BuildertrendMVC.Services
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(AppDbContext context, RoleManager<ApplicationRole> roleManager, IPermissionService permissionService)
        {
            // Crear roles base
            await CreateBaseRolesAsync(roleManager);

            var defaults = new (string Name, string Description, string Category)[]
            {
                ("quote:view", "Ver cotizaciones", "Quotes"),
                ("quote:create", "Crear cotizaciones", "Quotes"),
                ("quote:edit", "Editar cotizaciones", "Quotes"),
                ("quote:delete", "Eliminar cotizaciones", "Quotes"),
                ("quote:approve", "Aprobar cotizaciones", "Quotes"),
                ("quote:view-costs", "Ver costos y márgenes", "Quotes"),
                ("quote:sign", "Firmar cotizaciones", "Quotes"),
                ("quote:restore-version", "Restaurar versiones de cotización", "Quotes"),
            };

            foreach (var item in defaults)
            {
                await permissionService.CreatePermissionAsync(item.Name, item.Description, item.Category);
            }

            // Asignar permisos al rol Admin
            var admin = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (admin != null)
            {
                await permissionService.AssignPermissionsToRoleAsync(
                    admin.Id,
                    "quote:view",
                    "quote:create",
                    "quote:edit",
                    "quote:delete",
                    "quote:approve",
                    "quote:view-costs",
                    "quote:sign",
                    "quote:restore-version"
                );
            }

            // Asignar permisos al rol Empleado
            var empleado = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Empleado");
            if (empleado != null)
            {
                await permissionService.AssignPermissionsToRoleAsync(
                    empleado.Id,
                    "quote:view",
                    "quote:create",
                    "quote:edit",
                    "quote:view-costs",
                    "quote:sign"
                );
            }

            // Asignar permisos al rol Gerente
            var gerente = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Gerente");
            if (gerente != null)
            {
                await permissionService.AssignPermissionsToRoleAsync(
                    gerente.Id,
                    "quote:view",
                    "quote:approve",
                    "quote:view-costs"
                );
            }

            // Asignar permisos al rol Cliente
            var cliente = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Cliente");
            if (cliente != null)
            {
                await permissionService.AssignPermissionsToRoleAsync(
                    cliente.Id,
                    "quote:view",
                    "quote:sign"
                );
            }

            // Asignar permisos al rol Contador
            var contador = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == "Contador");
            if (contador != null)
            {
                await permissionService.AssignPermissionsToRoleAsync(
                    contador.Id,
                    "quote:view",
                    "quote:view-costs"
                );
            }
        }

        private static async Task CreateBaseRolesAsync(RoleManager<ApplicationRole> roleManager)
        {
            var rolesToCreate = new[]
            {
                "Admin",
                "Empleado",
                "Gerente",
                "Cliente",
                "Contador"
            };

            foreach (var roleName in rolesToCreate)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = roleName });
                }
            }
        }
    }
}
