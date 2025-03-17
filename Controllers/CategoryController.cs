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
    public class CategoryController : ControllerBase
    {
        private readonly EFCoreDBContext _context;

        public CategoryController(EFCoreDBContext context)
        {
            _context = context;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    IsActive = c.IsActive,  

                }).ToListAsync();

            return Ok(categories);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    IsActive = c.IsActive
                }).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [HttpGet("{id}/Events")]
        public async Task<ActionResult<IEnumerable<CategoryEventDto>>> GetEventsByCategory(int id)
        {
            var events = await _context.Events
                .Where(e => e.CategoryId == id)
                .Select(e => new CategoryEventDto
                {
                    EventId = e.EventId,
                    Title = e.Title,
                    Price = e.Price,
                    IsActive = e.IsActive,
                    CategoryId = e.CategoryId,
                    CategoryName = _context.Categories.Where(c => c.CategoryId == e.CategoryId).Select(c => c.Name).FirstOrDefault(),
                    VenueName=e.VenueName,
                    Duration=e.Duration,
                    StartTime=e.StartTime,
                    EndTime=e.EndTime
                }).ToListAsync();

            if (events == null || !events.Any())
            {
                return NotFound("No events found for the specified category.");
            }

            return Ok(events);
        }

        // POST: api/Category
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<Category>> CreateCategory(CategoryCreateUpdateDto categoryDto)
        {
            if (string.IsNullOrEmpty(categoryDto.Name))
            {
                return BadRequest("Please fill all fields to add a category.");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                IsActive = categoryDto.IsActive
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        // PUT: api/Category/5
        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryCreateUpdateDto categoryDto)
        {
            if (string.IsNullOrEmpty(categoryDto.Name))
            {
                return BadRequest("Please fill all fields to update the category.");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = categoryDto.Name;
            category.IsActive = categoryDto.IsActive;

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Category/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.IsActive = false;
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

