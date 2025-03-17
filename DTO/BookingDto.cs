namespace Eventhub.DTO;
public class BookingDto
{
    public int BookingId { get; set; }
    public string EventTitle { get; set; }
    public string VenueName { get; set; }
    public int UserId { get; set; }
    public DateTime BookingDate { get; set; }
    public int NoOfTickets { get; set; }
}


