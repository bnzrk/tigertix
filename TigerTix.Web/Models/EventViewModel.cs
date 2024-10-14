using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class EventViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int DateDay { get; set; }
        [Required]
        public int DateMonth { get; set; }
        [Required]
        public int DateYear { get; set; }
    }
}
