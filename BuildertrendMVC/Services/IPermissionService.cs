using System.Threading.Tasks;

namespace BuildertrendMVC.Services
{
    /// <summary>
    /// Interfaz para servicios de gestión de permisos
    /// </summary>
    public interface IPermissionService
    {
        /// <summary>
        /// Verificar si un usuario tiene alguno de los permisos requeridos
        /// </summary>
        Task<bool> UserHasPermissionAsync(string userId, params string[] permissions);

        /// <summary>
        /// Obtener todos los permisos de un usuario
        /// </summary>
        Task<IList<string>> GetUserPermissionsAsync(string userId);

        /// <summary>
        /// Obtener todos los permisos de un rol
        /// </summary>
        Task<IList<string>> GetRolePermissionsAsync(string roleId);

        /// <summary>
        /// Asignar permisos a un rol
        /// </summary>
        Task AssignPermissionsToRoleAsync(string roleId, params string[] permissionNames);

        /// <summary>
        /// Remover permisos de un rol
        /// </summary>
        Task RemovePermissionsFromRoleAsync(string roleId, params string[] permissionNames);

        /// <summary>
        /// Crear un nuevo permiso
        /// </summary>
        Task<int> CreatePermissionAsync(string name, string description, string category);

        /// <summary>
        /// Obtener todos los permisos disponibles
        /// </summary>
        Task<IList<Models.Permission>> GetAllPermissionsAsync();

        /// <summary>
        /// Obtener permisos por categoría
        /// </summary>
        Task<IList<Models.Permission>> GetPermissionsByCategoryAsync(string category);
    }
}
