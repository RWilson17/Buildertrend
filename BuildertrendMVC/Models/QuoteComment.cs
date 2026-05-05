using System;
using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class QuoteComment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
