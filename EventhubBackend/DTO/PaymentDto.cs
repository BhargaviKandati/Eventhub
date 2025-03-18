namespace Eventhub.DTO;
public class PaymentDto
{
    public int PaymentId { get; set; }
    public int TicketId { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Method { get; set; }
}
