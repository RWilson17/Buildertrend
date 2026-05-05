using System.ComponentModel.DataAnnotations;

namespace BuildertrendMVC.Models
{
    public class SettingsViewModel
    {
        [Display(Name = "Nombre de la empresa")]
        public string? CompanyName { get; set; }

        [Display(Name = "URL del logo")]
        public string? LogoUrl { get; set; }

        [Display(Name = "Color primario (hex)")]
        public string? PrimaryColor { get; set; }

        // Variables estáticas para simular persistencia en memoria
        public static string CurrentCompanyName { get; set; } = "Buildertrend";
        public static string CurrentLogoUrl { get; set; } = "https://cdn-icons-png.flaticon.com/512/1828/1828884.png";
        public static string CurrentPrimaryColor { get; set; } = "#0d6efd";
    }
}
