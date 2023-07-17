using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly StoreContext _context;
        public GenreController(StoreContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

            return Ok(genres);
        }

        [HttpPost]
        public async Task<ActionResult> CreateItem(GenreDto dto)
        {
            var genre = new Genre {Name = dto.Name};

            await _context.AddAsync(genre);
            _context.SaveChanges();

            return Ok(genre);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromBody] GenreDto dto)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if(genre == null)
                return NotFound($"No genre was found with id: {id}");
            
            genre.Name = dto.Name;

            _context.SaveChanges();

            return Ok(genre);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var genre = await _context.Genres.SingleOrDefaultAsync(g => g.Id == id);

            if(genre == null)
                return NotFound($"No genre was found with id: {id}");
            
            _context.Remove(genre);
            _context.SaveChanges();

            return Ok(genre);
        }
    }
}