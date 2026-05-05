using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildertrendMVC.Models
{
    public class QuoteItem
    {
        [Key]
        public int Id { get; set; }
        public string? CostCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal Margin { get; set; }
        public decimal CustomerCost { get; set; }
        public decimal TotalCost { get; set; }
        public string? CostType { get; set; }
        public decimal MarkupPercentage { get; set; }
        public int QuoteId { get; set; }
        [ForeignKey("QuoteId")]
        public Quote? Quote { get; set; }
    }
}
