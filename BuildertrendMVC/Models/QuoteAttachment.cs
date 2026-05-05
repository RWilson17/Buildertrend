using System;
using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class QuoteAttachment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; } = string.Empty;
        [Required]
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public int QuoteId { get; set; }
        public Quote? Quote { get; set; }
    }
}
