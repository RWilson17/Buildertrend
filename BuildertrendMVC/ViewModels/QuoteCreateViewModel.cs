using System.Collections.Generic;
using BuildertrendMVC.Models;

namespace BuildertrendMVC.ViewModels
{
    public class QuoteCreateViewModel
    {
        public int Id { get; set; }
        public string? SalesTax { get; set; }
        public string? QuoteNumber { get; set; }
        public List<QuoteItem> Items { get; set; } = new List<QuoteItem>();

        // Cliente relacionado
        public int? ClientId { get; set; }
        public List<Client>? ClientesDisponibles { get; set; }

        // Para carga de archivos adjuntos
        public List<IFormFile>? Attachments { get; set; }
        // Para mostrar archivos ya adjuntados (en edición)
        public List<QuoteAttachment>? ExistingAttachments { get; set; }

        // Estado de cotización
        public string? Estado { get; set; }
        public List<EstadoCotizacion>? EstadosDisponibles { get; set; }
    }
}
