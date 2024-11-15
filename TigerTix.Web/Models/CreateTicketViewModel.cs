using System.ComponentModel.DataAnnotations;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class CreateTicketViewModel
    {
        [Required]
        public int EventId { get; set; }
        public Ticket Ticket { get; set; } = new Ticket();
    }
}
