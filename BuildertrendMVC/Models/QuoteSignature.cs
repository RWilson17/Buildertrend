using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class QuoteSignature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteId { get; set; }

        [Required]
        [MaxLength(150)]
        public string SignerName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string SignerEmail { get; set; } = string.Empty;

        [Required]
        public DateTime SignedAt { get; set; } = DateTime.Now;

        [Required]
        public string SignatureHash { get; set; } = string.Empty;

        public string? IpAddress { get; set; }

        public bool AcceptedTerms { get; set; }

        public Quote? Quote { get; set; }
    }
}
