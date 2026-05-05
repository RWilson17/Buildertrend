using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BuildertrendMVC.Controllers
{
    public class EstadoCotizacionController : Controller
    {
        private readonly AppDbContext _context;
        public EstadoCotizacionController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var estados = await _context.EstadosCotizacion.OrderBy(e => e.Orden).ToListAsync();
            return View(estados);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EstadoCotizacion model)
        {
            if (ModelState.IsValid)
            {
                _context.EstadosCotizacion.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var estado = await _context.EstadosCotizacion.FindAsync(id);
            if (estado == null) return NotFound();
            return View(estado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EstadoCotizacion model)
        {
            if (ModelState.IsValid)
            {
                _context.EstadosCotizacion.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var estado = await _context.EstadosCotizacion.FindAsync(id);
            if (estado == null) return NotFound();
            _context.EstadosCotizacion.Remove(estado);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
