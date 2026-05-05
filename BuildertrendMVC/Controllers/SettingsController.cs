using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;

namespace BuildertrendMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            var model = new SettingsViewModel
            {
                CompanyName = SettingsViewModel.CurrentCompanyName,
                LogoUrl = SettingsViewModel.CurrentLogoUrl,
                PrimaryColor = SettingsViewModel.CurrentPrimaryColor
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                SettingsViewModel.CurrentCompanyName = model.CompanyName;
                SettingsViewModel.CurrentLogoUrl = model.LogoUrl;
                SettingsViewModel.CurrentPrimaryColor = model.PrimaryColor;
                TempData["SuccessMessage"] = "Configuración guardada.";
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
