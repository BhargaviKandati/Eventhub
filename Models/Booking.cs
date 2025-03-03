using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eventhub.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required] 
        public DateTime BookingDate { get; set; }
        public int NoOfTickets { get; set; }
    }
}