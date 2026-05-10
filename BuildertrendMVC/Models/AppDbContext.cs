
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BuildertrendMVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public IList<string> Roles { get; set; } = new List<string>();

        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string Currency { get; set; } = "USD";

        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string Language { get; set; } = "es";
    }

    public class ApplicationRole : IdentityRole
    {
        // Relación: Un rol puede tener múltiples permisos
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }

    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteItem> QuoteItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<QuoteAttachment> QuoteAttachments { get; set; }
        public DbSet<QuoteComment> QuoteComments { get; set; }
        public DbSet<EstadoCotizacion> EstadosCotizacion { get; set; }
        public DbSet<QuoteEvent> QuoteEvents { get; set; }
        public DbSet<Client> Clients { get; set; }
        
        // Nuevos DbSets para permisos
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Versionado y firma de cotizaciones
        public DbSet<QuoteVersion> QuoteVersions { get; set; }
        public DbSet<QuoteSignature> QuoteSignatures { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RolePermission>()
                .HasIndex(rp => new { rp.RoleId, rp.PermissionId })
                .IsUnique();

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuoteVersion>()
                .HasOne(v => v.Quote)
                .WithMany(q => q.Versions)
                .HasForeignKey(v => v.QuoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuoteSignature>()
                .HasOne(s => s.Quote)
                .WithOne(q => q.Signature)
                .HasForeignKey<QuoteSignature>(s => s.QuoteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<QuoteSignature>()
                .HasIndex(s => s.QuoteId)
                .IsUnique();
        }
    }
}

