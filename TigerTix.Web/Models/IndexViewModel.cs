using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class IndexViewModel
    {
        [Required]
        public string UserName { get; set; }
    }
}
