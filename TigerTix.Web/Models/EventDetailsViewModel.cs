using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class EventDetailsViewModel
    {
        [Required]
        public int EventId { get; set; }
        [Required]
        public EventViewModel Event { get; set; }
        public List<TicketViewModel> Tickets { get; set; } = new List<TicketViewModel>();
    }
}
