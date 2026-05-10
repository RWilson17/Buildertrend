using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class QuoteVersion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int QuoteId { get; set; }

        [Required]
        public int VersionNumber { get; set; }

        [Required]
        public string SnapshotJson { get; set; } = string.Empty;

        public string? ChangeSummary { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Quote? Quote { get; set; }
    }
}
