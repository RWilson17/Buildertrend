using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BuildertrendMVC.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(100)]
        public string? Empresa { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        [MaxLength(20)]
        public string? Telefono { get; set; }
        [MaxLength(200)]
        public string? Direccion { get; set; }
        // Relación 1 a muchos con cotizaciones
        public ICollection<Quote>? Quotes { get; set; }
    }
}
