using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BuildertrendMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Propiedad auxiliar para mostrar roles en la vista (no mapeada a BD)
        public IList<string> Roles { get; set; } = new List<string>();
    }

    public class ApplicationRole : IdentityRole
    {
    }

    public class AppIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
    }
}
