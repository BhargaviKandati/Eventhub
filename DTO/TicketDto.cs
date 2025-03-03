namespace Eventhub.DTOs
{
    public class TicketDto
    {
        public int TicketId { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
