using System;

namespace BuildertrendMVC.ViewModels
{
    public class GlobalSearchResultViewModel
    {
        public string Tipo { get; set; } // Cotización, Partida, Comentario, Archivo
        public int? Id { get; set; }
        public string? Folio { get; set; }
        public string? Estado { get; set; }
        public string? Texto { get; set; }
        public string? Extra { get; set; } // Info adicional (ej: nombre de archivo, usuario, etc)
        public DateTime? Fecha { get; set; }
    }
}