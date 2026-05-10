using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;

using System.Data;
using System.IO;
using BuildertrendMVC.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authorization;
namespace BuildertrendMVC.Controllers
{
    [Authorize]
    public class ExcelController : Controller
    {
        private readonly AppDbContext _context;

        public ExcelController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            int registros = 0;
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    file.CopyTo(stream);
                    using (var workbook = new XLWorkbook(stream))
                    {
                        var worksheet = workbook.Worksheets.First();
                        bool firstRow = true;
                        foreach (var row in worksheet.RowsUsed())
                        {
                            if (firstRow)
                            {
                                firstRow = false;
                                continue;
                            }
                            var cells = row.Cells().ToList();
                            // Si el Excel tiene 10 columnas, se asume formato Quote; si tiene 11, formato Product
                            if (cells.Count == 10)
                            {
                                // Crea una cotización con una sola partida
                                var quote = new Quote
                                {
                                    SalesTax = "0.07", // Valor por defecto, puedes ajustar
                                    Items = new List<QuoteItem>()
                                    {
                                        new QuoteItem {
                                            CostCode = cells[0].GetValue<string>(),
                                            Title = cells[1].GetValue<string>(),
                                            Description = cells[2].GetValue<string>(),
                                            Qty = cells[3].GetValue<decimal>(),
                                            UnitCost = cells[4].GetValue<decimal>(),
                                            Margin = cells[5].GetValue<decimal>(),
                                            CustomerCost = cells[6].GetValue<decimal>(),
                                            TotalCost = cells[7].GetValue<decimal>(),
                                            CostType = cells[8].GetValue<string>(),
                                            MarkupPercentage = cells[9].GetValue<decimal>()
                                        }
                                    }
                                };
                                _context.Quotes.Add(quote);
                                registros++;
                            }
                            else if (cells.Count >= 11)
                            {
                                var product = new Product
                                {
                                    Type = cells[0].GetValue<string>(),
                                    Model = cells[1].GetValue<string>(),
                                    Material = cells[2].GetValue<string>(),
                                    Color = cells[3].GetValue<string>(),
                                    Width = cells[4].GetValue<decimal>(),
                                    Height = cells[5].GetValue<decimal>(),
                                    Quantity = cells[6].GetValue<int>(),
                                    UnitCost = cells[7].GetValue<decimal>(),
                                    TotalCost = cells[8].GetValue<decimal>(),
                                    Description = cells[9].GetValue<string>()
                                };
                                _context.Products.Add(product);
                                registros++;
                            }
                        }
                        _context.SaveChanges();
                        ViewBag.Message = $"Archivo '{file.FileName}' cargado correctamente. Registros importados: {registros}.";
                    }
                }
            }
            else
            {
                ViewBag.Message = "Por favor selecciona un archivo válido.";
            }
            return View();
        }
    }
}
