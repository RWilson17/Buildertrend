
using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class Quote
    {
        [Display(Name = "Estado")]
        [Required]
        public string Estado { get; set; } = "Borrador"; // Borrador, Enviada, Aprobada, Cancelada
        [Key]
        public int Id { get; set; }
        [Required]
        public string QuoteNumber { get; set; } = string.Empty; // Folio único
        public decimal SalesTaxAmount { get; set; }
        [Required(ErrorMessage = "El campo Sales Tax es obligatorio.")]
        public string? SalesTax { get; set; }

        // Relación 1 a muchos con QuoteItem
        public ICollection<QuoteItem> Items { get; set; } = new List<QuoteItem>();

        [Display(Name = "Fecha de creación")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        // Relación 1 a muchos con archivos adjuntos
        public ICollection<QuoteAttachment> Attachments { get; set; } = new List<QuoteAttachment>();

        // Relación con cliente
        public int? ClientId { get; set; }
        public Client? Client { get; set; }
    }
}
