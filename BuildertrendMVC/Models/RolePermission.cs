namespace BuildertrendMVC.Models
{
    /// <summary>
    /// Relación de muchos-a-muchos entre Roles y Permisos
    /// </summary>
    public class RolePermission
    {
        public int Id { get; set; }

        // Claves foráneas
        public string RoleId { get; set; }
        public int PermissionId { get; set; }

        // Propiedades de navegación
        public virtual ApplicationRole Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
