using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class ProfileViewModel
    {
        [Required]
        [Display(Name = "CUID")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
