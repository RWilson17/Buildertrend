using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }
        
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }
        
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmPassword { get; set; }
        
        public List<string>? AllRoles { get; set; }
        public List<string>? SelectedRoles { get; set; }

        // Preferencias de usuario
        public string Currency { get; set; } = "USD";
        public string Language { get; set; } = "es";
    }
}

