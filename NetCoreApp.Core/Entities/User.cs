using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class User
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Se souvenir?")]
        public bool RememberMe { get; set; }

    }
}
