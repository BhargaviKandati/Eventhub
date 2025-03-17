using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventhub.Models;
using Eventhub.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using iText.Kernel.Pdf;
using iText.Layout.Element;
using iText.Layout;

namespace Eventhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly EFCoreDBContext _context;

        public TicketController(EFCoreDBContext context)
        {
            _context = context;
        }

        // POST: api/Ticket
        [HttpPost]
        //[Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Ticket>> PostTicket(TicketDto ticketDto)
        {
            var booking = await _context.Bookings.FindAsync(ticketDto.BookingId);
            var user = await _context.Users.FindAsync(ticketDto.UserId);
            if (booking == null || user == null)
            {
                return BadRequest("Booking or user not found.");
            }
            var ticket = new Ticket
            {
                UserId = ticketDto.UserId,
                PurchaseDate = ticketDto.PurchaseDate,
                BookingId = ticketDto.BookingId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.TicketId }, ticket);
        }

        // GET: api/Ticket
        [HttpGet]
        //[Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // Method to generate a PDF for a given Ticket
        private byte[] GenerateTicketPdf(Ticket ticket, String venue, Event @event, Booking booking)
        {
            using (var ms = new MemoryStream())
            {
                var writer = new PdfWriter(ms); // Create a PdfWriter to write to the memory stream
                var pdf = new PdfDocument(writer); // Create a PdfDocument
                var document = new Document(pdf); // Create a Document to add elements to the PDF

                // Add ticket details to the PDF
                document.Add(new Paragraph($"Ticket for {ticket.User.FullName}"));
                document.Add(new Paragraph($"Booking ID: {ticket.BookingId}"));
                document.Add(new Paragraph($"Purchase Date: {ticket.PurchaseDate:dd-MM-yyyy}"));

                // Add venue details to the PDF
                document.Add(new Paragraph($"Venue: {venue}"));

                // Add event details to the PDF
                document.Add(new Paragraph($"Event: {@event.Title}"));
                document.Add(new Paragraph($"Start Time: {@event.StartTime:dd-MM-yyyy HH:mm}"));
                document.Add(new Paragraph($"End Time: {@event.EndTime:dd-MM-yyyy HH:mm}"));

                // Add booking details to the PDF
                document.Add(new Paragraph($"Number of Tickets: {booking.NoOfTickets}"));
                document.Add(new Paragraph($"Booking Date: {booking.BookingDate:dd-MM-yyyy}"));

                // Calculate and add price details to the PDF
                var totalPrice = booking.NoOfTickets * @event.Price;
                document.Add(new Paragraph($"Total Price: {totalPrice:C}"));

                document.Close(); // Close the document
                return ms.ToArray(); // Return the PDF as a byte array
            }
        }

        // Endpoint to generate a PDF for a ticket
        [HttpPost("generate")]
        //[Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> GeneratePdf([FromBody] TicketRequest request)
        {
            // Fetch the ticket from the database, including related Booking, User, Event, and Venue entities
            var ticket = await _context.Tickets
                .Include(t => t.Booking)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.TicketId == request.TicketId);

            if (ticket == null)
            {
                return BadRequest("Ticket not found."); // Return an error if the ticket is not found
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.BookingId == ticket.BookingId);

            if (booking == null)
            {
                return BadRequest("Booking not found."); // Return an error if the booking is not found
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(e => e.EventId == booking.EventId);

            if (@event == null)
            {
                return BadRequest("Event not found."); // Return an error if the event is not found
            }

            var venue = Convert.ToString(@event.VenueName);

            if (venue == null)
            {
                return BadRequest("Venue not found."); // Return an error if the venue is not found
            }

            // Generate the PDF for the ticket
            var pdfBytes = GenerateTicketPdf(ticket, venue, @event, booking);
            return File(pdfBytes, "application/pdf", "ticket.pdf"); // Return the PDF as a file
        }
    }

    public class TicketRequest
    {
        public int TicketId { get; set; }
    }
}



