using System.Collections.Generic;

namespace BuildertrendMVC.Models
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public List<string>? AllRoles { get; set; }
        public List<string>? SelectedRoles { get; set; }
    }
}
