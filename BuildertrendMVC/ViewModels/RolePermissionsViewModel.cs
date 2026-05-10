using BuildertrendMVC.Models;

namespace BuildertrendMVC.ViewModels
{
    public class RolePermissionsViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public IList<Permission> AllPermissions { get; set; } = new List<Permission>();
        public IList<string> SelectedPermissionNames { get; set; } = new List<string>();
    }
}
