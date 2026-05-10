
using Microsoft.AspNetCore.Mvc;
using System.IO;
using BuildertrendMVC.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuildertrendMVC.ViewModels;
using BuildertrendMVC.Attributes;
using BuildertrendMVC.Services;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
namespace BuildertrendMVC.Controllers
{
    [Authorize]
    public class QuoteController : Controller
    {


        [HttpGet]
        public async Task<IActionResult> Exportar(int id, string plantilla = "PlantillaClasica")
        {
            var quote = await _context.Quotes.Include(q => q.Items).FirstOrDefaultAsync(q => q.Id == id);
            if (quote == null) return NotFound();
            // Renderizar la plantilla seleccionada
            var viewPath = $"~/Templates/{plantilla}.cshtml";
            var html = await RazorTemplateToString(viewPath, quote);
            // Convertir HTML a PDF (usando SelectPdf, DinkToPdf, o similar)
            // Aquí solo se devuelve HTML para simplificar
            return Content(html, "text/html");
        }

        // Renderizar Razor a string
        private async Task<string> RazorTemplateToString(string viewPath, object model)
        {
            using var sw = new StringWriter();
            var viewResult = Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.Found(viewPath, null);
            var tempDataProvider = HttpContext.RequestServices.GetService(typeof(Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider)) as Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataProvider;
            var viewContext = new Microsoft.AspNetCore.Mvc.Rendering.ViewContext(
                ControllerContext,
                viewResult.View,
                new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<object>(
                    metadataProvider: new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    modelState: ModelState)
                {
                    Model = model
                },
                new Microsoft.AspNetCore.Mvc.ViewFeatures.TempDataDictionary(HttpContext, tempDataProvider),
                sw,
                new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions()
            );
            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
        private readonly AppDbContext _context;
        private readonly Services.AuditService _auditService;
        private readonly Services.EmailService _emailService;
        private readonly IPermissionService _permissionService;
        public QuoteController(AppDbContext context, Services.AuditService auditService, Services.EmailService emailService, IPermissionService permissionService)
        {
            _context = context;
            _auditService = auditService;
            _emailService = emailService;
            _permissionService = permissionService;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var quote = await _context.Quotes
                .Include(q => q.Items)
                .Include(q => q.Attachments)
                .Include(q => q.Signature)
                .FirstOrDefaultAsync(q => q.Id == id);
            // Obtener preferencias del usuario
            string currency = "USD", language = "es";
            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    currency = user.Currency ?? "USD";
                    language = user.Language ?? "es";
                }
            }
            ViewBag.Currency = currency;
            ViewBag.Language = language;
            ViewBag.CanViewCosts = await CurrentUserHasPermissionAsync("quote:view-costs");
            ViewBag.CanSign = await CurrentUserHasPermissionAsync("quote:sign");
            ViewBag.CanRestoreVersions = await CurrentUserHasPermissionAsync("quote:restore-version");
            if (quote == null) return NotFound();
            // Obtener historial de cambios
            var audit = await _context.AuditLogs
                .Where(a => a.EntityName == "Quote" && a.EntityId == quote.Id.ToString())
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
            ViewBag.AuditLogs = audit;
            // Obtener comentarios
            var comments = await _context.QuoteComments
                .Where(c => c.QuoteId == quote.Id)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            ViewBag.QuoteComments = comments;
            // Obtener eventos de calendario
            var events = await _context.QuoteEvents.Where(e => e.QuoteId == quote.Id).OrderBy(e => e.Date).ToListAsync();
            ViewBag.QuoteEvents = events;
            var versions = await _context.QuoteVersions
                .Where(v => v.QuoteId == quote.Id)
                .OrderByDescending(v => v.VersionNumber)
                .ToListAsync();
            ViewBag.QuoteVersions = versions;
            return View(quote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEvent(int quoteId, string title, DateTime date, string description, string color)
        {
            if (!User.Identity.IsAuthenticated || !(User.IsInRole("Admin") || User.IsInRole("Empleado")))
            {
                TempData["ErrorMessage"] = "No tienes permisos para agregar eventos.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                TempData["ErrorMessage"] = "El título del evento es obligatorio.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null)
            {
                TempData["ErrorMessage"] = "Cotización no encontrada.";
                return RedirectToAction("Index");
            }
            var newEvent = new QuoteEvent
            {
                QuoteId = quoteId,
                Title = title,
                Date = date,
                Description = description,
                Color = string.IsNullOrWhiteSpace(color) ? "#007bff" : color
            };
            _context.QuoteEvents.Add(newEvent);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Evento agregado correctamente.";
            return RedirectToAction("Details", new { id = quoteId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int eventId, int quoteId)
        {
            if (!User.Identity.IsAuthenticated || !(User.IsInRole("Admin") || User.IsInRole("Empleado")))
            {
                TempData["ErrorMessage"] = "No tienes permisos para eliminar eventos.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            var ev = await _context.QuoteEvents.FirstOrDefaultAsync(e => e.Id == eventId && e.QuoteId == quoteId);
            if (ev == null)
            {
                TempData["ErrorMessage"] = "Evento no encontrado.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            _context.QuoteEvents.Remove(ev);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Evento eliminado correctamente.";
            return RedirectToAction("Details", new { id = quoteId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int quoteId, string comment)
        {
            if (!User.Identity.IsAuthenticated || !(User.IsInRole("Admin") || User.IsInRole("Empleado")))
            {
                TempData["ErrorMessage"] = "No tienes permisos para agregar comentarios.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            if (string.IsNullOrWhiteSpace(comment))
            {
                TempData["ErrorMessage"] = "El comentario no puede estar vacío.";
                return RedirectToAction("Details", new { id = quoteId });
            }
            var quote = await _context.Quotes.FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null)
            {
                TempData["ErrorMessage"] = "Cotización no encontrada.";
                return RedirectToAction("Index");
            }
            var userName = User.Identity?.Name ?? "Usuario";
            var newComment = new Models.QuoteComment
            {
                QuoteId = quoteId,
                UserName = userName,
                Comment = comment,
                CreatedAt = DateTime.Now
            };
            _context.QuoteComments.Add(newComment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Comentario agregado correctamente.";
            return RedirectToAction("Details", new { id = quoteId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var quote = await _context.Quotes.Include(q => q.Items).Include(q => q.Attachments).FirstOrDefaultAsync(q => q.Id == id);
            if (quote == null) return NotFound();
            var estados = await _context.EstadosCotizacion.Where(e => e.EsActivo).OrderBy(e => e.Orden).ToListAsync();
            var clientes = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
            var vm = new ViewModels.QuoteCreateViewModel
            {
                Id = quote.Id,
                SalesTax = quote.SalesTax,
                QuoteNumber = quote.QuoteNumber,
                Items = quote.Items != null ? quote.Items.ToList() : new List<Models.QuoteItem>(),
                ExistingAttachments = quote.Attachments?.ToList() ?? new List<QuoteAttachment>(),
                Estado = quote.Estado,
                EstadosDisponibles = estados,
                ClientId = quote.ClientId,
                ClientesDisponibles = clientes
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(QuoteCreateViewModel vm)
        {
            if (vm == null || vm.Id == 0) return NotFound();
            var quote = await _context.Quotes.Include(q => q.Items).Include(q => q.Attachments).FirstOrDefaultAsync(q => q.Id == vm.Id);
            if (quote == null) return NotFound();

            // Validación y actualización de campos
            decimal salesTaxValue = 0;
            string salesTaxStr = (vm.SalesTax ?? "").Replace(',', '.');
            if (!decimal.TryParse(salesTaxStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out salesTaxValue))
            {
                ModelState.AddModelError("SalesTax", "El valor de Sales Tax no es válido.");
            }
            decimal[] salesTaxValid = { 0.07m, 0.0725m, 0.0775m };
            bool isValidSalesTax = salesTaxValid.Any(v => Math.Abs(v - salesTaxValue) < 0.0001m);
            if (!isValidSalesTax)
                ModelState.AddModelError("SalesTax", "El valor seleccionado para Sales Tax no es válido.");
            if (vm.Items == null || vm.Items.Count == 0)
                ModelState.AddModelError("Items", "Debes agregar al menos una partida.");
            // Recalcular campos automáticos
            if (vm.Items != null)
            {
                foreach (var item in vm.Items)
                {
                    var margin = item.Margin;
                    if (margin > 1m)
                    {
                        margin /= 100m;
                    }
                    margin = Math.Min(Math.Max(margin, 0m), 0.99m);
                    item.Margin = margin;
                    var divisor = 1m - margin;
                    item.CustomerCost = divisor > 0m ? (item.UnitCost / divisor) : 0m;
                    item.MarkupPercentage = (margin < 1m && margin > 0m) ? (margin / (1m - margin)) * 100m : 0m;
                    item.TotalCost = item.Qty * item.CustomerCost;
                }
            }
            // Validar estado solo si usuario es interno
            if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Empleado")))
            {
                var estados = await _context.EstadosCotizacion.Where(e => e.EsActivo).OrderBy(e => e.Orden).ToListAsync();
                if (string.IsNullOrEmpty(vm.Estado) || !estados.Any(e => e.Nombre == vm.Estado))
                {
                    ModelState.AddModelError("Estado", "Selecciona un estado válido.");
                }
                if (vm.Estado == "Aprobada" && !await CurrentUserHasPermissionAsync("quote:approve"))
                {
                    ModelState.AddModelError("Estado", "No tienes permisos para aprobar cotizaciones.");
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await CreateVersionSnapshotAsync(quote, "Respaldo previo a edición");

                    // Historial de cambios por campo
                    var cambios = new List<string>();
                    // Cotización
                    if (quote.SalesTax != vm.SalesTax)
                        cambios.Add($"SalesTax: '{quote.SalesTax}' → '{vm.SalesTax}'");
                    if (quote.SalesTaxAmount != vm.Items.Where(x => x.CostType == "Material").Sum(x => x.TotalCost * salesTaxValue))
                        cambios.Add($"SalesTaxAmount: '{quote.SalesTaxAmount}' → '{vm.Items.Where(x => x.CostType == "Material").Sum(x => x.TotalCost * salesTaxValue)}'");
                    // Estado y notificación
                    if (User.Identity.IsAuthenticated && (User.IsInRole("Admin") || User.IsInRole("Empleado")))
                    {
                        if (quote.Estado != vm.Estado)
                        {
                            cambios.Add($"Estado: '{quote.Estado}' → '{vm.Estado}'");
                            string estadoAnterior = quote.Estado;
                            quote.Estado = vm.Estado;
                            // Notificar por correo
                            try
                            {
                                // Obtener correo del cliente si existe
                                string destinatario = null;
                                if (quote.ClientId.HasValue)
                                {
                                    var cliente = await _context.Clients.FindAsync(quote.ClientId.Value);
                                    destinatario = cliente?.Email;
                                }
                                if (string.IsNullOrWhiteSpace(destinatario))
                                    destinatario = "cliente@correo.com";
                                string asunto = $"Cotización {quote.QuoteNumber} actualizada: Estado cambiado";
                                string cuerpo = $"<p>La cotización <b>{quote.QuoteNumber}</b> ha cambiado de estado:</p>"
                                    + $"<ul><li><b>Anterior:</b> {estadoAnterior}</li><li><b>Nuevo:</b> {vm.Estado}</li></ul>"
                                    + $"<p>Para más detalles, accede al sistema.</p>";
                                _ = _emailService.SendEmailAsync(destinatario, asunto, cuerpo);
                            }
                            catch { /* Ignorar errores de notificación para no bloquear el guardado */ }
                        }
                    }
                    // Partidas
                    var oldItems = quote.Items.ToList();
                    var newItems = vm.Items;
                    if (oldItems.Count != newItems.Count)
                        cambios.Add($"Partidas: cantidad {oldItems.Count} → {newItems.Count}");
                    else
                    {
                        for (int i = 0; i < oldItems.Count; i++)
                        {
                            var oldItem = oldItems[i];
                            var newItem = newItems[i];
                            if (oldItem.CostCode != newItem.CostCode)
                                cambios.Add($"Partida[{i}].CostCode: '{oldItem.CostCode}' → '{newItem.CostCode}'");
                            if (oldItem.Title != newItem.Title)
                                cambios.Add($"Partida[{i}].Title: '{oldItem.Title}' → '{newItem.Title}'");
                            if (oldItem.Description != newItem.Description)
                                cambios.Add($"Partida[{i}].Description: '{oldItem.Description}' → '{newItem.Description}'");
                            if (oldItem.Qty != newItem.Qty)
                                cambios.Add($"Partida[{i}].Qty: '{oldItem.Qty}' → '{newItem.Qty}'");
                            if (oldItem.UnitCost != newItem.UnitCost)
                                cambios.Add($"Partida[{i}].UnitCost: '{oldItem.UnitCost}' → '{newItem.UnitCost}'");
                            if (oldItem.CustomerCost != newItem.CustomerCost)
                                cambios.Add($"Partida[{i}].CustomerCost: '{oldItem.CustomerCost}' → '{newItem.CustomerCost}'");
                            if (oldItem.Margin != newItem.Margin)
                                cambios.Add($"Partida[{i}].Margin: '{oldItem.Margin}' → '{newItem.Margin}'");
                            if (oldItem.MarkupPercentage != newItem.MarkupPercentage)
                                cambios.Add($"Partida[{i}].MarkupPercentage: '{oldItem.MarkupPercentage}' → '{newItem.MarkupPercentage}'");
                            if (oldItem.TotalCost != newItem.TotalCost)
                                cambios.Add($"Partida[{i}].TotalCost: '{oldItem.TotalCost}' → '{newItem.TotalCost}'");
                            if (oldItem.CostType != newItem.CostType)
                                cambios.Add($"Partida[{i}].CostType: '{oldItem.CostType}' → '{newItem.CostType}'");
                        }
                    }
                    // Adjuntos
                    var oldAtt = quote.Attachments?.ToList() ?? new List<QuoteAttachment>();
                    var filesAdded = vm.Attachments?.Count ?? 0;
                    if (filesAdded > 0)
                        cambios.Add($"Adjuntos: +{filesAdded} archivo(s) nuevo(s)");

                    // Actualizar campos principales
                    quote.SalesTax = vm.SalesTax;
                    quote.Items = vm.Items;
                    quote.SalesTaxAmount = vm.Items.Where(x => x.CostType == "Material").Sum(x => x.TotalCost * salesTaxValue);
                    // Relación con cliente
                    quote.ClientId = vm.ClientId;
                    // Manejar archivos adjuntos nuevos
                    if (vm.Attachments != null && vm.Attachments.Count > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                        foreach (var file in vm.Attachments)
                        {
                            if (file != null && file.Length > 0)
                            {
                                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                var attachment = new QuoteAttachment
                                {
                                    FileName = file.FileName,
                                    FilePath = uniqueFileName,
                                    UploadedAt = DateTime.Now,
                                    QuoteId = quote.Id
                                };
                                _context.Add(attachment);
                            }
                        }
                    }
                    await _context.SaveChangesAsync();
                    // Registrar historial de cambios
                    if (cambios.Count > 0)
                        _auditService.Log("Quote", quote.Id.ToString(), "Edit", string.Join("; ", cambios));
                    TempData["SuccessMessage"] = "Cotización actualizada correctamente.";
                    return RedirectToAction("Details", new { id = quote.Id });
                }
                catch (Exception ex)
                {
                    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                    var logFile = Path.Combine(logPath, "error.log");
                    System.IO.File.AppendAllText(logFile, $"[{DateTime.Now}] Error al editar cotización: {ex.Message}\n{ex.StackTrace}\n");
                    TempData["ErrorMessage"] = "Error inesperado al guardar la cotización. Consulta el log de errores.";
                    // Recargar adjuntos existentes
                    vm.ExistingAttachments = quote.Attachments?.ToList() ?? new List<QuoteAttachment>();
                    // Recargar clientes y estados disponibles
                    vm.ClientesDisponibles = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
                    vm.EstadosDisponibles = await _context.EstadosCotizacion.Where(e => e.EsActivo).OrderBy(e => e.Orden).ToListAsync();
                    return View(vm);
                }
            }
            // Recargar adjuntos existentes si hay error
            vm.ExistingAttachments = quote.Attachments?.ToList() ?? new List<QuoteAttachment>();
            // Recargar clientes y estados disponibles
            vm.ClientesDisponibles = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
            vm.EstadosDisponibles = await _context.EstadosCotizacion.Where(e => e.EsActivo).OrderBy(e => e.Orden).ToListAsync();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var clientes = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
            var vm = new QuoteCreateViewModel
            {
                Items = new List<QuoteItem>(),
                ClientesDisponibles = clientes
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuoteCreateViewModel vm)
        {
            // LOGGING TEMPORAL PARA DEPURACIÓN
            var logPathDebug = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            if (!Directory.Exists(logPathDebug)) Directory.CreateDirectory(logPathDebug);
            var logFileDebug = Path.Combine(logPathDebug, "debug.log");
            try
            {
                System.IO.File.AppendAllText(logFileDebug, $"[{DateTime.Now}] POST Create: Items.Count={vm.Items?.Count ?? 0}\n");
                if (vm.Items != null)
                {
                    foreach (var item in vm.Items)
                    {
                        System.IO.File.AppendAllText(logFileDebug, $"  CostType={item.CostType}, Qty={item.Qty}, UnitCost={item.UnitCost}, Margin={item.Margin}, CustomerCost={item.CustomerCost}, TotalCost={item.TotalCost}\n");
                    }
                }
            }
            catch {}

            decimal salesTaxValue = 0;
            string salesTaxStr = (vm.SalesTax ?? "").Replace(',', '.');
            if (!decimal.TryParse(salesTaxStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out salesTaxValue))
            {
                ModelState.AddModelError("SalesTax", "El valor de Sales Tax no es válido.");
            }
            decimal[] salesTaxValid = { 0.07m, 0.0725m, 0.0775m };
            bool isValidSalesTax = salesTaxValid.Any(v => Math.Abs(v - salesTaxValue) < 0.0001m);
            if (!isValidSalesTax)
                ModelState.AddModelError("SalesTax", "El valor seleccionado para Sales Tax no es válido.");

            // Validación de partidas
            if (vm.Items == null || vm.Items.Count == 0)
                ModelState.AddModelError("Items", "Debes agregar al menos una partida.");

            // Recalcular campos automáticos
            if (vm.Items != null)
            {
                foreach (var item in vm.Items)
                {
                    var margin = item.Margin;
                    if (margin > 1m)
                    {
                        margin /= 100m;
                    }
                    margin = Math.Min(Math.Max(margin, 0m), 0.99m);
                    item.Margin = margin;
                    var divisor = 1m - margin;
                    item.CustomerCost = divisor > 0m ? (item.UnitCost / divisor) : 0m;
                    item.MarkupPercentage = (margin < 1m && margin > 0m) ? (margin / (1m - margin)) * 100m : 0m;
                    item.TotalCost = item.Qty * item.CustomerCost;
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Generar QuoteNumber único: Q-YYYYMMDD-XXXX
                    var today = DateTime.Now.Date;
                    var countToday = _context.Quotes.Count(q => EF.Functions.Like(q.QuoteNumber, $"Q-{today:yyyyMMdd}-%")) + 1;
                    var quoteNumber = $"Q-{today:yyyyMMdd}-{countToday:D4}";

                    var quote = new Quote
                    {
                        QuoteNumber = quoteNumber,
                        SalesTax = vm.SalesTax,
                        SalesTaxAmount = 0, // Se calcula abajo
                        Items = vm.Items,
                        DateCreated = DateTime.Now,
                        ClientId = vm.ClientId
                    };
                    // Calcula SalesTaxAmount sumando solo partidas tipo Material
                    quote.SalesTaxAmount = quote.Items.Where(x => x.CostType == "Material").Sum(x => x.TotalCost * salesTaxValue);
                    // Guardar primero la cotización para obtener el Id
                    _context.Add(quote);
                    await _context.SaveChangesAsync();

                    // Manejar archivos adjuntos
                    if (vm.Attachments != null && vm.Attachments.Count > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                        if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);
                        foreach (var file in vm.Attachments)
                        {
                            if (file != null && file.Length > 0)
                            {
                                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                var attachment = new QuoteAttachment
                                {
                                    FileName = file.FileName,
                                    FilePath = uniqueFileName,
                                    UploadedAt = DateTime.Now,
                                    QuoteId = quote.Id
                                };
                                _context.Add(attachment);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }

                    _auditService.Log("Quote", quote.Id.ToString(), "Create", $"Cotización creada con {quote.Items.Count} partidas, folio: {quote.QuoteNumber}");
                    await CreateVersionSnapshotAsync(quote, "Versión inicial");
                    TempData["SuccessMessage"] = $"Cotización creada exitosamente. Folio: {quote.QuoteNumber}";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Loguear error a archivo
                    var logPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
                    if (!Directory.Exists(logPath)) Directory.CreateDirectory(logPath);
                    var logFile = Path.Combine(logPath, "error.log");
                    System.IO.File.AppendAllText(logFile, $"[{DateTime.Now}] Error al crear cotización: {ex.Message}\n{ex.StackTrace}\n");
                    TempData["ErrorMessage"] = "Error inesperado al guardar la cotización. Consulta el log de errores.";
                    // Recargar clientes disponibles
                    vm.ClientesDisponibles = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
                    return View(vm);
                }
            }
            // Recargar clientes disponibles si hay error
            vm.ClientesDisponibles = await _context.Clients.OrderBy(c => c.Nombre).ToListAsync();
            return View(vm);
        }
        // Acción Index para mostrar la lista de cotizaciones
        public async Task<IActionResult> Index()
        {
            var quotes = await _context.Quotes.Include(q => q.Items).OrderByDescending(q => q.Id).ToListAsync();
            // Obtener preferencias del usuario
            string currency = "USD", language = "es";
            if (User.Identity.IsAuthenticated)
            {
                var userManager = HttpContext.RequestServices.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;
                var user = await userManager.GetUserAsync(User);
                if (user != null)
                {
                    currency = user.Currency ?? "USD";
                    language = user.Language ?? "es";
                }
            }
            ViewBag.Currency = currency;
            ViewBag.Language = language;
            ViewBag.CanViewCosts = await CurrentUserHasPermissionAsync("quote:view-costs");
            return View(quotes);
        }

        // Acción para duplicar cotización
        [HttpGet]
        public async Task<IActionResult> Duplicate(int id)
        {
            var original = await _context.Quotes.Include(q => q.Items).FirstOrDefaultAsync(q => q.Id == id);
            if (original == null) return NotFound();
            // Generar nuevo folio
            var today = DateTime.Now.Date;
            var countToday = _context.Quotes.Count(q => EF.Functions.Like(q.QuoteNumber, $"Q-{today:yyyyMMdd}-%")) + 1;
            var quoteNumber = $"Q-{today:yyyyMMdd}-{countToday:D4}";
            // Clonar cotización
            var newQuote = new Quote
            {
                QuoteNumber = quoteNumber,
                SalesTax = original.SalesTax,
                SalesTaxAmount = original.SalesTaxAmount,
                Estado = "Borrador",
                DateCreated = DateTime.Now,
                Items = original.Items.Select(item => new QuoteItem
                {
                    CostCode = item.CostCode,
                    Title = item.Title,
                    Qty = item.Qty,
                    UnitCost = item.UnitCost,
                    CustomerCost = item.CustomerCost,
                    Margin = item.Margin,
                    MarkupPercentage = item.MarkupPercentage,
                    TotalCost = item.TotalCost,
                    CostType = item.CostType
                }).ToList()
            };
            _context.Quotes.Add(newQuote);
            await _context.SaveChangesAsync();
            await CreateVersionSnapshotAsync(newQuote, "Versión inicial (duplicada)");
            TempData["SuccessMessage"] = $"Cotización duplicada exitosamente. Folio: {newQuote.QuoteNumber}";
            return RedirectToAction("Index");
        }

        [Permission("quote:restore-version")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreVersion(int quoteId, int versionId)
        {
            var quote = await _context.Quotes.Include(q => q.Items).FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null) return NotFound();

            var version = await _context.QuoteVersions.FirstOrDefaultAsync(v => v.Id == versionId && v.QuoteId == quoteId);
            if (version == null) return NotFound();

            var snapshot = JsonSerializer.Deserialize<QuoteSnapshot>(version.SnapshotJson);
            if (snapshot == null)
            {
                TempData["ErrorMessage"] = "No se pudo restaurar la versión seleccionada.";
                return RedirectToAction("Details", new { id = quoteId });
            }

            await CreateVersionSnapshotAsync(quote, "Respaldo antes de restauración");

            quote.SalesTax = snapshot.SalesTax;
            quote.Estado = snapshot.Estado;
            quote.ClientId = snapshot.ClientId;

            _context.QuoteItems.RemoveRange(quote.Items);
            quote.Items = snapshot.Items.Select(i => new QuoteItem
            {
                CostCode = i.CostCode,
                Title = i.Title,
                Description = i.Description,
                Qty = i.Qty,
                UnitCost = i.UnitCost,
                Margin = i.Margin,
                CustomerCost = i.CustomerCost,
                TotalCost = i.TotalCost,
                CostType = i.CostType,
                MarkupPercentage = i.MarkupPercentage,
            }).ToList();

            var salesTaxValue = TryParseSalesTax(quote.SalesTax);
            quote.SalesTaxAmount = quote.Items.Where(x => x.CostType == "Material").Sum(x => x.TotalCost * salesTaxValue);

            await _context.SaveChangesAsync();
            _auditService.Log("Quote", quote.Id.ToString(), "RestoreVersion", $"Restaurada versión #{version.VersionNumber}");

            TempData["SuccessMessage"] = $"Versión #{version.VersionNumber} restaurada correctamente.";
            return RedirectToAction("Details", new { id = quoteId });
        }

        [Permission("quote:sign")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Sign(int quoteId, string signerName, string signerEmail, bool acceptedTerms)
        {
            var quote = await _context.Quotes.Include(q => q.Signature).FirstOrDefaultAsync(q => q.Id == quoteId);
            if (quote == null) return NotFound();

            if (quote.Signature != null)
            {
                TempData["ErrorMessage"] = "Esta cotización ya fue firmada.";
                return RedirectToAction("Details", new { id = quoteId });
            }

            if (string.IsNullOrWhiteSpace(signerName) || string.IsNullOrWhiteSpace(signerEmail))
            {
                TempData["ErrorMessage"] = "Nombre y correo del firmante son obligatorios.";
                return RedirectToAction("Details", new { id = quoteId });
            }

            if (!acceptedTerms)
            {
                TempData["ErrorMessage"] = "Debes aceptar la declaración de firma electrónica.";
                return RedirectToAction("Details", new { id = quoteId });
            }

            var raw = $"{quoteId}|{signerName.Trim()}|{signerEmail.Trim()}|{DateTime.UtcNow:O}";
            var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)));

            var signature = new QuoteSignature
            {
                QuoteId = quoteId,
                SignerName = signerName.Trim(),
                SignerEmail = signerEmail.Trim(),
                AcceptedTerms = true,
                SignatureHash = hash,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                SignedAt = DateTime.Now,
            };

            _context.QuoteSignatures.Add(signature);

            if (await CurrentUserHasPermissionAsync("quote:approve"))
            {
                quote.Estado = "Aprobada";
            }

            await _context.SaveChangesAsync();
            _auditService.Log("Quote", quote.Id.ToString(), "Sign", $"Firma electrónica registrada por {signature.SignerName}");

            TempData["SuccessMessage"] = "Firma electrónica registrada correctamente.";
            return RedirectToAction("Details", new { id = quoteId });
        }

        // Acción para eliminar cotización
        [Permission("quote:delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var quote = await _context.Quotes.Include(q => q.Items).FirstOrDefaultAsync(q => q.Id == id);
            if (quote == null) return NotFound();
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cotización eliminada correctamente.";
            return RedirectToAction("Index");
        }

        private async Task<bool> CurrentUserHasPermissionAsync(string permission)
        {
            if (User.IsInRole("Admin")) return true;

            var hasRoleClaim = User.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Role);
            if (!hasRoleClaim)
            {
                // Compatibilidad con instalaciones existentes sin roles explícitos.
                return permission != "quote:approve"
                    && permission != "quote:delete"
                    && permission != "quote:restore-version";
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId)) return false;
            return await _permissionService.UserHasPermissionAsync(userId, permission);
        }

        private static decimal TryParseSalesTax(string? salesTax)
        {
            if (string.IsNullOrWhiteSpace(salesTax)) return 0m;
            var value = salesTax.Replace(',', '.');
            return decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : 0m;
        }

        private async Task CreateVersionSnapshotAsync(Quote quote, string changeSummary)
        {
            var nextVersion = (await _context.QuoteVersions
                .Where(v => v.QuoteId == quote.Id)
                .MaxAsync(v => (int?)v.VersionNumber) ?? 0) + 1;

            var snapshot = new QuoteSnapshot
            {
                QuoteNumber = quote.QuoteNumber,
                SalesTax = quote.SalesTax,
                Estado = quote.Estado,
                ClientId = quote.ClientId,
                Items = quote.Items.Select(i => new QuoteItemSnapshot
                {
                    CostCode = i.CostCode,
                    Title = i.Title,
                    Description = i.Description,
                    Qty = i.Qty,
                    UnitCost = i.UnitCost,
                    Margin = i.Margin,
                    CustomerCost = i.CustomerCost,
                    TotalCost = i.TotalCost,
                    CostType = i.CostType,
                    MarkupPercentage = i.MarkupPercentage,
                }).ToList()
            };

            var version = new QuoteVersion
            {
                QuoteId = quote.Id,
                VersionNumber = nextVersion,
                SnapshotJson = JsonSerializer.Serialize(snapshot),
                ChangeSummary = changeSummary,
                CreatedBy = User.Identity?.Name ?? "Sistema",
                CreatedAt = DateTime.Now,
            };

            _context.QuoteVersions.Add(version);
            await _context.SaveChangesAsync();
        }

        private sealed class QuoteSnapshot
        {
            public string QuoteNumber { get; set; } = string.Empty;
            public string? SalesTax { get; set; }
            public string Estado { get; set; } = "Borrador";
            public int? ClientId { get; set; }
            public List<QuoteItemSnapshot> Items { get; set; } = new();
        }

        private sealed class QuoteItemSnapshot
        {
            public string? CostCode { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public decimal Qty { get; set; }
            public decimal UnitCost { get; set; }
            public decimal Margin { get; set; }
            public decimal CustomerCost { get; set; }
            public decimal TotalCost { get; set; }
            public string? CostType { get; set; }
            public decimal MarkupPercentage { get; set; }
        }
    }
}
