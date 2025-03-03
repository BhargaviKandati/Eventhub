using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Eventhub.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)]
        public string Method { get; set; }

        [Required]
        public int TicketId{ get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

    }
}
