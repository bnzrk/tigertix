using System.ComponentModel.DataAnnotations;

namespace TigerTix.Web.Models
{
    public class EventViewModel
    {
        [Required]
        public string EventName { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EventDate { get; set; }
    }
}
