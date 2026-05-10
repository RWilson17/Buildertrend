namespace BuildertrendMVC.Models
{
    /// <summary>
    /// Modelo para representar permisos granulares en el sistema
    /// </summary>
    public class Permission
    {
        public int Id { get; set; }
        
        /// <summary>Nombre único del permiso</summary>
        public string Name { get; set; }
        
        /// <summary>Descripción legible del permiso</summary>
        public string Description { get; set; }
        
        /// <summary>Categoría del permiso (Quotes, Products, Users, etc.)</summary>
        public string Category { get; set; }

        // Relación: Un permiso puede estar asignado a múltiples roles
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
