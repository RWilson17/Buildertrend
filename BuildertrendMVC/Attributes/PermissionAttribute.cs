using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BuildertrendMVC.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BuildertrendMVC.Attributes
{
    /// <summary>
    /// Atributo personalizado para verificar permisos granulares
    /// Uso: [Permission("quote:create", "quote:edit")]
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _permissions;

        public PermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Si no hay permisos especificados, permitir acceso
            if (_permissions == null || _permissions.Length == 0)
                return;

            var user = context.HttpContext.User;

            // Si no está autenticado, denegar acceso
            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new ForbidResult();
                return;
            }

            // Los administradores siempre tienen acceso
            if (user.IsInRole("Admin"))
                return;

            var hasRoleClaim = user.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Role);
            if (!hasRoleClaim)
            {
                // Compatibilidad con usuarios antiguos sin roles explícitos.
                var blocks = new[] { "quote:approve", "quote:delete", "quote:restore-version" };
                var shouldBlock = _permissions.Any(p => blocks.Contains(p));
                if (!shouldBlock)
                {
                    return;
                }
            }

            // Verificar si el usuario tiene al menos uno de los permisos requeridos
            var permissionService = context.HttpContext.RequestServices.GetService(typeof(IPermissionService)) as IPermissionService;
            if (permissionService == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var userId = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new ForbidResult();
                return;
            }
            var hasPermission = await permissionService.UserHasPermissionAsync(userId, _permissions);

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
