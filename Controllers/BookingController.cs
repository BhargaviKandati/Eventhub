using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventhub.Models;
using Eventhub.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Eventhub.DTO;

namespace Eventhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly EFCoreDBContext _context;
        
        public BookingController(EFCoreDBContext context)
        {
            _context = context;
        }

        // GET: api/Booking
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDto>>> GetBookings()
        {
            var bookings = await (from b in _context.Bookings
                                  join e in _context.Events on b.EventId equals e.EventId
                                  select new BookingDto
                                  {
                                      BookingId = b.BookingId,
                                      EventTitle = e.Title,
                                      VenueName = e.VenueName,
                                      BookingDate = b.BookingDate,
                                      NoOfTickets=b.NoOfTickets
                                  }).ToListAsync();

            return Ok(bookings);
        }

        // GET: api/Booking/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookingDto>> GetBooking(int id)
        {
            var booking = await (from b in _context.Bookings
                                 join e in _context.Events on b.EventId equals e.EventId
                                 where b.BookingId == id
                                 select new BookingDto
                                 {
                                     BookingId = b.BookingId,
                                     EventTitle = e.Title,
                                     VenueName = e.VenueName,
                                     BookingDate = b.BookingDate,
                                     UserId = b.UserId,
                                     NoOfTickets = b.NoOfTickets
                                     
                                 }).FirstOrDefaultAsync();

            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        // GET: api/Booking/EventsByDateRange
        [HttpGet("EventsByDateRange")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByDateRange(DateTime startDate, DateTime endDate)
        {
            var events = await _context.Events
                .Where(e => e.StartTime >= startDate && e.EndTime <= endDate)
                .Select(e => new EventDto
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    Price = e.Price,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    CategoryName = _context.Categories.Where(c => c.CategoryId == e.CategoryId).Select(c => c.Name).FirstOrDefault(),
                    VenueName = e.VenueName,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Duration = e.EndTime - e.StartTime
                }).ToListAsync();

            if (events == null || !events.Any())
            {
                return NotFound("No events found within the specified date range.");
            }

            return Ok(events);
        }

        // POST: api/Booking
        [HttpPost]
        //[Authorize(Roles = "User")]
        public async Task<ActionResult<Booking>> CreateBooking(BookingCreateUpdateDto bookingDto)
        {
            if (bookingDto.EventId <= 0)
            {
                return BadRequest("Please provide valid EventId.");
            }

            var Event = await _context.Events.FindAsync(bookingDto.EventId);
            var NoOfTickets = await _context.Bookings.FindAsync(bookingDto.NoOfTickets);
            if (Event == null)
            {
                return BadRequest("Event not found.");
            }

            var booking = new Booking
            {
                EventId = bookingDto.EventId,
                NoOfTickets=bookingDto.NoOfTickets,
                BookingDate = bookingDto.BookingDate,
                UserId = bookingDto.UserId
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
        }

        // PUT: api/Booking/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBooking(int id, BookingCreateUpdateDto bookingDto)
        {
            if (id != bookingDto.BookingId)
            {
                return BadRequest();
            }

            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            booking.EventId = bookingDto.EventId;
            booking.BookingDate = bookingDto.BookingDate;

            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Booking/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}