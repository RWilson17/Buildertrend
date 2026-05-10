using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;

using Microsoft.AspNetCore.Authorization;
namespace BuildertrendMVC.Controllers;

[Authorize]
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
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("/Identity/Account/Login");
        }
        ViewBag.TotalProductos = _context.Products?.Count() ?? 0;
        ViewBag.TotalCotizaciones = _context.Quotes?.Count() ?? 0;
        // Sumar los TotalCost de todos los QuoteItems
        var quoteItems = _context.QuoteItems?.ToList() ?? new List<BuildertrendMVC.Models.QuoteItem>();
        ViewBag.TotalVentas = quoteItems.Sum(qi => qi.TotalCost);
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
