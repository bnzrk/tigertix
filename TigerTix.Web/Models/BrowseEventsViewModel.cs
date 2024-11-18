using System.ComponentModel.DataAnnotations;
using TigerTix.Web.Data.Entities;

namespace TigerTix.Web.Models
{
    public class BrowseEventsViewModel
    {
        public EventSearchParameters SearchParameters { get; set; } = new EventSearchParameters();
        public IEnumerable<Event> Events { get; set; } = new List<Event>();
    }
}