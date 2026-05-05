using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;
using System.Linq;

namespace BuildertrendMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditController : Controller
    {
        private readonly AppDbContext _context;
        public AuditController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var logs = _context.AuditLogs.OrderByDescending(a => a.Timestamp).Take(100).ToList();
            return View(logs);
        }
    }
}
