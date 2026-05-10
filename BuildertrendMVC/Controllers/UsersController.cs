using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BuildertrendMVC.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BuildertrendMVC.Controllers
{
        [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<ApplicationUser>();
            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                user.Roles = roles.ToList();
                userList.Add(user);
            }
            return View(userList);
        }

        // Crear nuevo usuario (GET)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var model = new UserEditViewModel
            {
                AllRoles = allRoles,
                SelectedRoles = new List<string>(),
                Currency = "USD",
                Language = "es"
            };
            return View(model);
        }

        // Crear nuevo usuario (POST)
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserEditViewModel model, string[] SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.SelectedRoles = SelectedRoles.ToList();
                return View(model);
            }

            // Validar que el email sea único
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "Este email ya está registrado.");
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.SelectedRoles = SelectedRoles.ToList();
                return View(model);
            }

            // Crear nuevo usuario
            var newUser = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Currency = model.Currency,
                Language = model.Language
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
                model.SelectedRoles = SelectedRoles.ToList();
                return View(model);
            }

            // Asignar roles
            if (SelectedRoles != null && SelectedRoles.Length > 0)
            {
                await _userManager.AddToRolesAsync(newUser, SelectedRoles);
            }

            TempData["SuccessMessage"] = $"Usuario {model.Email} creado exitosamente.";
            return RedirectToAction("Index");
        }

        // Acción para que el usuario edite su propio perfil
        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return NotFound();
            
            // Obtener roles del usuario
            var userRoles = await _userManager.GetRolesAsync(currentUser);
            
            var model = new UserEditViewModel
            {
                Id = currentUser.Id,
                Email = currentUser.Email,
                AllRoles = new List<string>(), // No puede cambiar roles
                SelectedRoles = userRoles.ToList(),
                Currency = string.IsNullOrEmpty(currentUser.Currency) ? "USD" : currentUser.Currency,
                Language = string.IsNullOrEmpty(currentUser.Language) ? "es" : currentUser.Language
            };
            return View("Edit", model);
        }

        // Acción POST para que el usuario edite su propio perfil
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Users/PerfilSave")]
        public async Task<IActionResult> PerfilSave(UserEditViewModel model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return NotFound();

            // Validar que el usuario esté editando su propio perfil
            if (currentUser.Id != model.Id)
                return Forbid();

            // Guardar preferencias de moneda e idioma
            currentUser.Currency = model.Currency;
            currentUser.Language = model.Language;
            var result = await _userManager.UpdateAsync(currentUser);
            
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToAction("Perfil");
            }

            TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
            return RedirectToAction("Perfil");
        }

        // Solo administradores pueden editar a otros usuarios
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            var model = new UserEditViewModel
            {
                Id = user.Id,
                Email = user.Email,
                AllRoles = allRoles,
                SelectedRoles = userRoles.ToList(),
                Currency = user.Currency,
                Language = user.Language
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model, string[] SelectedRoles)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null) return NotFound();

            // Si es admin, puede cambiar roles
            if (User.IsInRole("Admin"))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var rolesToAdd = SelectedRoles.Except(userRoles);
                var rolesToRemove = userRoles.Except(SelectedRoles);
                await _userManager.AddToRolesAsync(user, rolesToAdd);
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            // Guardar preferencias de moneda e idioma
            user.Currency = model.Currency;
            user.Language = model.Language;
            await _userManager.UpdateAsync(user);

            TempData["SuccessMessage"] = "Perfil actualizado correctamente.";
            // Si es admin, regresa al listado; si es usuario normal, regresa al perfil
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index");
            else
                return RedirectToAction("Perfil");
        }

        // Eliminar usuario
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // No permitir eliminar al usuario actual
            var currentUser = await _userManager.GetUserAsync(User);
            if (user.Id == currentUser.Id)
            {
                TempData["ErrorMessage"] = "No puedes eliminar tu propio usuario.";
                return RedirectToAction("Index");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = $"Usuario {user.Email} eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el usuario.";
            }

            return RedirectToAction("Index");
        }
    }
}
