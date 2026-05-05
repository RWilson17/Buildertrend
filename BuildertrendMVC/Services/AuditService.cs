using System;
using BuildertrendMVC.Models;
using Microsoft.AspNetCore.Http;

namespace BuildertrendMVC.Services
{
    public class AuditService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Log(string entity, string entityId, string action, string changes)
        {
            var user = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anon";
            var log = new AuditLog
            {
                EntityName = entity,
                EntityId = entityId,
                Action = action,
                UserName = user,
                Timestamp = DateTime.Now,
                Changes = changes
            };
            _context.AuditLogs.Add(log);
            _context.SaveChanges();
        }
    }
}
