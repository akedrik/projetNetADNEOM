using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class UserLogin : User
    {
        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Se souvenir?")]
        public bool RememberMe { get; set; }

    }
}
