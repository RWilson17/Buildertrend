using Microsoft.EntityFrameworkCore;

namespace BuildertrendMVC.Models
{
    public class AppDbContext : DbContext
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
    }
}
