using System.ComponentModel.DataAnnotations;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class TicketViewModel
    {
        [Required]
        public int CUID { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
