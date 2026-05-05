using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class EstadoCotizacion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Descripcion { get; set; }
        public bool EsActivo { get; set; } = true;
        public int Orden { get; set; } = 0;
    }
}
