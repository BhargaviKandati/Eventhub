using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventhub.Models;
using Eventhub.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Eventhub.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eventhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly EFCoreDBContext _context;

        public PaymentController(EFCoreDBContext context)
        {
            _context = context;
        }

        // POST: api/Payment
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Payment>> PostPayment(PaymentDto paymentDto)
        {
            // Check if the TicketId exists in the Ticket table
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.TicketId == paymentDto.TicketId);
            if (ticket == null)
            {
                return BadRequest("Invalid TicketId. The ticket does not exist.");
            }

            // Get the BookingId from the Ticket
            int bookingId = ticket.BookingId;

            // Find the Booking and get the NoOfTickets and EventId
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            if (booking == null)
            {
                return BadRequest("Invalid BookingId. The booking does not exist.");
            }

            int noOfTickets = booking.NoOfTickets;
            int eventId = booking.EventId;

            // Find the Event and get the Price
            var eventEntity = await _context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);
            if (eventEntity == null)
            {
                return BadRequest("Invalid EventId. The event does not exist.");
            }

            float eventPrice = eventEntity.Price;

            var payment = new Payment
            {
                TicketId = paymentDto.TicketId,
                Amount = (decimal)eventPrice*noOfTickets,
                PaymentDate = paymentDto.PaymentDate,
                Method = paymentDto.Method
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPayment), new { id = payment.PaymentId }, payment);
        }

 

        // GET: api/Payment
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            try
            {
                return await _context.Payments.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }


        // GET: api/Payment/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound();
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework)
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}

