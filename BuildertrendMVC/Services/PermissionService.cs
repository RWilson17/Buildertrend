using BuildertrendMVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildertrendMVC.Services
{
    /// <summary>
    /// Servicio para gestionar permisos granulares por rol
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;

        public PermissionService(AppDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, params string[] permissions)
        {
            if (string.IsNullOrEmpty(userId) || permissions == null || permissions.Length == 0)
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Obtener roles del usuario
            var userRoles = await _userManager.GetRolesAsync(user);

            // Obtener permisos de todos los roles del usuario
            var userPermissions = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            // Verificar si tiene al menos uno de los permisos requeridos
            return permissions.Any(p => userPermissions.Contains(p));
        }

        public async Task<IList<string>> GetUserPermissionsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return new List<string>();

            var userRoles = await _userManager.GetRolesAsync(user);

            var permissions = await _context.RolePermissions
                .Where(rp => userRoles.Contains(rp.Role.Name))
                .Select(rp => rp.Permission.Name)
                .Distinct()
                .ToListAsync();

            return permissions;
        }

        public async Task<IList<string>> GetRolePermissionsAsync(string roleId)
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .Select(rp => rp.Permission.Name)
                .ToListAsync();

            return permissions;
        }

        public async Task AssignPermissionsToRoleAsync(string roleId, params string[] permissionNames)
        {
            foreach (var permName in permissionNames)
            {
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == permName);

                if (permission != null)
                {
                    // Verificar si la relación ya existe
                    var existing = await _context.RolePermissions
                        .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

                    if (existing == null)
                    {
                        _context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = roleId,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionsFromRoleAsync(string roleId, params string[] permissionNames)
        {
            foreach (var permName in permissionNames)
            {
                var permission = await _context.Permissions
                    .FirstOrDefaultAsync(p => p.Name == permName);

                if (permission != null)
                {
                    var rolePermission = await _context.RolePermissions
                        .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permission.Id);

                    if (rolePermission != null)
                    {
                        _context.RolePermissions.Remove(rolePermission);
                    }
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreatePermissionAsync(string name, string description, string category)
        {
            // Verificar que el permiso no exista ya
            var existing = await _context.Permissions
                .FirstOrDefaultAsync(p => p.Name == name);

            if (existing != null)
                return existing.Id;

            var permission = new Permission
            {
                Name = name,
                Description = description,
                Category = category
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();

            return permission.Id;
        }

        public async Task<IList<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .OrderBy(p => p.Category)
                .ThenBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IList<Permission>> GetPermissionsByCategoryAsync(string category)
        {
            return await _context.Permissions
                .Where(p => p.Category == category)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }
    }
}
