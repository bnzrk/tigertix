using System.ComponentModel.DataAnnotations;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class UserTicketViewModel
    {
        [Required]
        public string Section { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public string EventName { get; set; }
    }
}
