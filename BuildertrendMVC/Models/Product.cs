using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string? Type { get; set; } // Window or Door
        public string? Model { get; set; }
        public string? Material { get; set; }
        public string? Color { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string? Description { get; set; }
    }
}
