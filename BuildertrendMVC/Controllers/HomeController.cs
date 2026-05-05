using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;

namespace BuildertrendMVC.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _context;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var totalProductos = _context.Products.Count();
        var totalCotizaciones = _context.Quotes.Count();
        // Sumar CustomerCost de todas las partidas de todas las cotizaciones
        var quotes = _context.Quotes.ToList();
        decimal totalVentas = 0;
        foreach (var q in quotes)
        {
            if (q.Items != null)
                totalVentas += q.Items.Sum(i => i.CustomerCost);
        }

        ViewBag.TotalProductos = totalProductos;
        ViewBag.TotalCotizaciones = totalCotizaciones;
        ViewBag.TotalVentas = totalVentas;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
