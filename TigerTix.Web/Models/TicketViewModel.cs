using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class TicketViewModel
    {
        public int CUID { get; set; }
        [Required]
        public string TicketEvent { get; set; }
        [Required]
        public int SeatNumber { get; set; }
    }
}
