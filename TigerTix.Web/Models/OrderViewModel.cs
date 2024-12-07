using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class OrderViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public List<OrderTicketViewModel> Tickets { get; set; } = new List<OrderTicketViewModel>();
    }
}
