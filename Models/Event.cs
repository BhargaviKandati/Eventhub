using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventhub.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int VenueId { get; set; }
        public TimeSpan Duration => EndTime - StartTime;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

}