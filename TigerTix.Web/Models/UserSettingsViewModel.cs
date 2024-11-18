using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class UserSettingsViewModel
    {
        [Display(Name = "Account Recovery Phone Number")]
        public string PhoneNumber { get; set; }
    }
}