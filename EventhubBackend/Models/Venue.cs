using System.ComponentModel.DataAnnotations;

namespace Eventhub.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(300)]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }

    }
}
