﻿namespace Eventhub.DTO
{
    public class BookingCreateUpdateDto
    {
        public int BookingId { get; set; }
        public int EventId { get; set; }
        public int VenueId { get; set; }
        public DateTime BookingDate { get; set; }
        public int NoOfTickets { get; set; }
    }
}
