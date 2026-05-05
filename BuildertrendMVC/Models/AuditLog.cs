using System;

namespace BuildertrendMVC.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public string? EntityId { get; set; }
        public string? Action { get; set; }
        public string? UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Changes { get; set; }
    }
}
