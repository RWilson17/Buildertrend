using System;
using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class QuoteEvent
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; } = "#007bff";
    }
}
