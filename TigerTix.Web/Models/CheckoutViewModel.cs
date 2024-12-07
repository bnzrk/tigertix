using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int OrderId { get; set; }
        // TODO: Payment info fields
    }
}
