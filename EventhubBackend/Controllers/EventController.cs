using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventhub.Models;
using Eventhub.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Eventhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EFCoreDBContext _context;

        public EventController(EFCoreDBContext context)
        {
            _context = context;
        }

        // GET: api/Event
        [HttpGet]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await (from e in _context.Events
                                join c in _context.Categories on e.CategoryId equals c.CategoryId
                                join v in _context.Venues on e.VenueId equals v.VenueId
                                select new EventDto
                                {
                                    EventId = e.EventId,
                                    Title = e.Title,
                                    Price = e.Price,
                                    IsActive = e.IsActive,
                                    CategoryId=c.CategoryId,
                                    CategoryName = c.Name,
                                    VenueName = v.Name,
                                    VenueId=v.VenueId,
                                    StartTime = e.StartTime,
                                    EndTime = e.EndTime,
                                    Duration = e.EndTime - e.StartTime
                                }).ToListAsync();

            return Ok(events);
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventItem = await (from e in _context.Events
                                   join c in _context.Categories on e.CategoryId equals c.CategoryId
                                   join v in _context.Venues on e.VenueId equals v.VenueId
                                   where e.EventId == id
                                   select new EventDto
                                   {
                                       EventId = e.EventId,
                                       Title = e.Title,
                                       Price = e.Price,
                                       IsActive = e.IsActive,
                                       CategoryName = c.Name,
                                       VenueName = v.Name,
                                       StartTime = e.StartTime,
                                       EndTime = e.EndTime,
                                       Duration = e.EndTime - e.StartTime
                                   }).FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
        }

        // POST: api/Event
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Event>> CreateEvent(EventCreateUpdateDto eventDto)
        {
            if (string.IsNullOrEmpty(eventDto.Title))
            {
                return BadRequest("Please fill all fields to add an event.");
            }

            var existingEventByTitle = await _context.Events
                .Where(e => e.Title == eventDto.Title)
                .FirstOrDefaultAsync();

            if (existingEventByTitle != null)
            {
                return BadRequest("Event with the same title already exists.");
            }

            var eventItem = new Event
            {
                Title = eventDto.Title,
                Price = eventDto.Price,
                IsActive = eventDto.IsActive,
                CategoryId = eventDto.CategoryId,
                VenueId = eventDto.VenueId,
                StartTime = eventDto.StartTime,
                EndTime = eventDto.EndTime
            };

            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = eventItem.EventId }, eventItem);
        }

        // PUT: api/Event/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(int id, EventCreateUpdateDto eventDto)
        {
            if (string.IsNullOrEmpty(eventDto.Title))
            {
                return BadRequest("Please fill all fields to update the event.");
            }

            if (id != eventDto.EventId)
            {
                return BadRequest();
            }

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.Title = eventDto.Title;
            eventItem.Price = eventDto.Price;
            eventItem.IsActive = eventDto.IsActive;
            eventItem.CategoryId = eventDto.CategoryId;
            eventItem.VenueId = eventDto.VenueId;
            eventItem.StartTime = eventDto.StartTime;
            eventItem.EndTime = eventDto.EndTime;

            _context.Entry(eventItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Event/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
