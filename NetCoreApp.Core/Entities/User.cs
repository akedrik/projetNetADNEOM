using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public abstract class User
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public virtual string Email { get; set; }
    }
}
