using System.ComponentModel.DataAnnotations;

namespace NetCoreApp.Core.Entities
{
    public class UserExternalLogin
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
