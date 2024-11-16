using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class TicketViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Section { get; set; }
        [Required]
        public int Row { get; set; }
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
