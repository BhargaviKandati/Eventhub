using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eventhub.Models;
using Eventhub.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Eventhub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly EFCoreDBContext _context; 
        private readonly ILogger<VenueController> _logger;

        public VenueController(EFCoreDBContext context, ILogger<VenueController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Venue
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenueDto>>> GetVenues()
        {
            //log info can be viewed in console output
            _logger.LogInformation("Getting all venues");
            var venues = await _context.Venues
                .Select(v => new VenueDto
                {
                    VenueId = v.VenueId,
                    Name = v.Name,
                    Location = v.Location,
                    Capacity = v.Capacity
                }).ToListAsync();

            return Ok(venues);
        }

        // GET: api/Venue/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VenueDto>> GetVenue(int id)
        {
            _logger.LogInformation("Getting venues by Id");
            var venue = await _context.Venues
                .Where(v => v.VenueId == id)
                .Select(v => new VenueDto
                {
                    VenueId = v.VenueId,
                    Name = v.Name,
                    Location = v.Location,
                    Capacity = v.Capacity
                }).FirstOrDefaultAsync();

            if (venue == null)
            {
               
                return NotFound();
            }

            return Ok(venue);
        }

        // POST: api/Venue
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Venue>> CreateVenue(VenueDto venueDto)
        {
            if (string.IsNullOrEmpty(venueDto.Name))
            {
                return BadRequest("Please fill all fields to add a venue.");
            }
            _logger.LogInformation("Creating venue");
            var venue = new Venue
            {
                Name = venueDto.Name,
                Location = venueDto.Location,
                Capacity = venueDto.Capacity
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVenue), new { id = venue.VenueId }, venue);
        }

        // PUT: api/Venue/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateVenue(int id, VenueDto venueDto)
        {
            if (string.IsNullOrEmpty(venueDto.Name))
            {
                return BadRequest("Please fill all fields to update the venue.");
            }

            if (id != venueDto.VenueId)
            {
                return BadRequest();
            }

            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Updating venue");
            venue.Name = venueDto.Name;
            venue.Location = venueDto.Location;
            venue.Capacity = venueDto.Capacity;

            _context.Entry(venue).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Venue/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            _logger.LogInformation("Deleting venue");
            var venue = await _context.Venues.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }

            _context.Venues.Remove(venue);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

