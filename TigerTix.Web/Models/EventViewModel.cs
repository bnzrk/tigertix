using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class EventViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        public decimal BasePrice { get; set; }
    }
}
