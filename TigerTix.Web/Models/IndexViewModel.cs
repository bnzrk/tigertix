using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class IndexViewModel
    {
        [Required]
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
