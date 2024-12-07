using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class OrderTicketViewModel
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
        public DateTime EventDate { get; set; }
        [Required]
        public string EventName { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
