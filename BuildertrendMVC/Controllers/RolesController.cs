using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;
using BuildertrendMVC.Services;
using BuildertrendMVC.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BuildertrendMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPermissionService _permissionService;

        public RolesController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IPermissionService permissionService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _permissionService = permissionService;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole { Name = name });
                if (result.Succeeded)
                    return RedirectToAction("Index");
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
                await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Permissions(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var allPermissions = await _permissionService.GetAllPermissionsAsync();
            var selected = await _permissionService.GetRolePermissionsAsync(id);

            var model = new RolePermissionsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name ?? string.Empty,
                AllPermissions = allPermissions,
                SelectedPermissionNames = selected,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Permissions(RolePermissionsViewModel model, string[] selectedPermissions)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null) return NotFound();

            var current = await _permissionService.GetRolePermissionsAsync(model.RoleId);
            var selected = selectedPermissions?.ToList() ?? new List<string>();

            var toAdd = selected.Except(current).ToArray();
            var toRemove = current.Except(selected).ToArray();

            if (toAdd.Length > 0)
                await _permissionService.AssignPermissionsToRoleAsync(model.RoleId, toAdd);

            if (toRemove.Length > 0)
                await _permissionService.RemovePermissionsFromRoleAsync(model.RoleId, toRemove);

            TempData["SuccessMessage"] = "Permisos del rol actualizados correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
