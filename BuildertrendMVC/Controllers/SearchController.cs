using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using BuildertrendMVC.Models;
using BuildertrendMVC.ViewModels;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
namespace BuildertrendMVC.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        public SearchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string q)
        {
            ViewBag.Query = q;
            if (string.IsNullOrWhiteSpace(q))
                return View(new List<GlobalSearchResultViewModel>());
            q = q.ToLower();
            var results = new List<GlobalSearchResultViewModel>();

            // Cotizaciones
            var quotes = await _context.Quotes.Include(x => x.Items).Include(x => x.Attachments).ToListAsync();
            results.AddRange(
                quotes.Where(c =>
                    (c.QuoteNumber != null && c.QuoteNumber.ToLower().Contains(q)) ||
                    (c.Estado != null && c.Estado.ToLower().Contains(q)) ||
                    (c.Items != null && c.Items.Any(i => (i.Title != null && i.Title.ToLower().Contains(q)) || (i.Description != null && i.Description.ToLower().Contains(q))))
                ).Select(c => new GlobalSearchResultViewModel
                {
                    Tipo = "Cotización",
                    Id = c.Id,
                    Folio = c.QuoteNumber,
                    Estado = c.Estado,
                    Texto = $"Cotización {c.QuoteNumber} - Estado: {c.Estado}",
                    Fecha = c.DateCreated
                })
            );

            // Partidas
            foreach (var c in quotes)
            {
                if (c.Items == null) continue;
                results.AddRange(
                    c.Items.Where(i =>
                        (i.Title != null && i.Title.ToLower().Contains(q)) ||
                        (i.Description != null && i.Description.ToLower().Contains(q))
                    ).Select(i => new GlobalSearchResultViewModel
                    {
                        Tipo = "Partida",
                        Id = c.Id,
                        Folio = c.QuoteNumber,
                        Estado = c.Estado,
                        Texto = $"{i.Title} ({i.Description})",
                        Extra = $"Cotización: {c.QuoteNumber}",
                        Fecha = c.DateCreated
                    })
                );
            }

            // Comentarios
            var comments = await _context.QuoteComments.ToListAsync();
            results.AddRange(
                comments.Where(com => com.Comment != null && com.Comment.ToLower().Contains(q))
                .Select(com => new GlobalSearchResultViewModel
                {
                    Tipo = "Comentario",
                    Id = com.QuoteId,
                    Folio = quotes.FirstOrDefault(c => c.Id == com.QuoteId)?.QuoteNumber,
                    Texto = com.Comment,
                    Extra = $"Por: {com.UserName}",
                    Fecha = com.CreatedAt
                })
            );

            // Archivos adjuntos
            var attachments = quotes.SelectMany(c => c.Attachments.Select(a => new { a, c })).ToList();
            results.AddRange(
                attachments.Where(x => x.a.FileName != null && x.a.FileName.ToLower().Contains(q))
                .Select(x => new GlobalSearchResultViewModel
                {
                    Tipo = "Archivo",
                    Id = x.c.Id,
                    Folio = x.c.QuoteNumber,
                    Texto = x.a.FileName,
                    Extra = $"Cotización: {x.c.QuoteNumber}",
                    Fecha = x.a.UploadedAt
                })
            );

            // Ordenar por fecha descendente
            results = results.OrderByDescending(r => r.Fecha).ToList();
            return View(results);
        }
    }
}
