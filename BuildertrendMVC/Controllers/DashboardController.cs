using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using BuildertrendMVC.Models;
using BuildertrendMVC.ViewModels;

namespace BuildertrendMVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string estado, string producto, DateTime? desde, DateTime? hasta)
        {
            var cotizaciones = await _context.Quotes.Include(q => q.Items).ToListAsync();
            // Filtrar en memoria para evitar problemas con SQLite y decimal
            if (!string.IsNullOrEmpty(estado))
                cotizaciones = cotizaciones.Where(q => q.Estado == estado).ToList();
            if (!string.IsNullOrEmpty(producto))
                cotizaciones = cotizaciones.Where(q => q.Items.Any(i => i.Title != null && i.Title.Contains(producto))).ToList();
            if (desde.HasValue)
                cotizaciones = cotizaciones.Where(q => q.DateCreated >= desde).ToList();
            if (hasta.HasValue)
                cotizaciones = cotizaciones.Where(q => q.DateCreated <= hasta).ToList();

            var lista = cotizaciones.Select(q => new DashboardQuoteViewModel {
                QuoteNumber = q.QuoteNumber,
                Estado = q.Estado,
                DateCreated = q.DateCreated,
                Total = q.Items?.Sum(i => (decimal)(i.TotalCost)) ?? 0
            }).ToList();
            ViewBag.Cotizaciones = lista;
            ViewBag.Estados = await _context.EstadosCotizacion.Select(e => e.Nombre).Distinct().ToListAsync();
            ViewBag.Productos = await _context.QuoteItems.Select(i => i.Title).Distinct().ToListAsync();

            // Construir la URL de exportación con los parámetros actuales
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(estado)) query["estado"] = estado;
            if (!string.IsNullOrEmpty(producto)) query["producto"] = producto;
            if (desde.HasValue) query["desde"] = desde.Value.ToString("yyyy-MM-dd");
            if (hasta.HasValue) query["hasta"] = hasta.Value.ToString("yyyy-MM-dd");
            var exportUrl = Url.Action("Exportar", "Dashboard") + (query.Count > 0 ? ("?" + query.ToString()) : "");
            ViewBag.ExportUrl = exportUrl;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Exportar(string estado, string producto, DateTime? desde, DateTime? hasta)
        {
            var cotizaciones = _context.Quotes.Include(q => q.Items).AsQueryable();
            if (!string.IsNullOrEmpty(estado))
                cotizaciones = cotizaciones.Where(q => q.Estado == estado);
            if (!string.IsNullOrEmpty(producto))
                cotizaciones = cotizaciones.Where(q => q.Items.Any(i => i.Title.Contains(producto)));
            if (desde.HasValue)
                cotizaciones = cotizaciones.Where(q => q.DateCreated >= desde);
            if (hasta.HasValue)
                cotizaciones = cotizaciones.Where(q => q.DateCreated <= hasta);

            var lista = await cotizaciones.ToListAsync();
            var csv = "Folio,Estado,Fecha,Total\n";
            foreach (var c in lista)
            {
                var total = c.Items?.Sum(i => i.TotalCost) ?? 0;
                csv += $"{c.QuoteNumber},{c.Estado},{c.DateCreated:yyyy-MM-dd},{total}\n";
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
            return File(bytes, "text/csv", "reporte_cotizaciones.csv");
        }
    }
}
