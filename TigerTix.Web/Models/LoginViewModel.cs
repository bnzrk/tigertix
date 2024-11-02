using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; } = false;
    }
}
