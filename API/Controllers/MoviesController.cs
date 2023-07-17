using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _context;
        private new List<String> _allowedEx = new List<string> {".jpg", ".png"};
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(StoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllItem()
        {
            var movies = await _context.Movies
            .OrderByDescending(x => x.Rate)
            .Include(m => m.Genre)
            .Select(m => new MovieDetailsDto 
            {
                Id = m.Id,
                GenreId = m.GenreId,
                GenreName = m.Genre.Name,
                Poster = m.Poster,
                Rate = m.Rate,
                Storyline = m.Storyline,
                Title = m.Title,
                Year = m.Year
            })
            .ToListAsync();

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            
            if(movie == null)
                return NotFound();

            var dto = _mapper.Map<MovieDetailsDto>(movie);
            
            return Ok(dto);
        }

        [HttpGet("GetByGenreId")]
        public async Task<ActionResult> GetByGenreId(byte genreId)
        {
            var movies = await _context.Movies
            
            .Where(m => m.GenreId == genreId)
            .OrderByDescending(x => x.Rate)
            .Include(m => m.Genre)
            .Select(m => new MovieDetailsDto 
            {
                Id = m.Id,
                GenreId = m.GenreId,
                GenreName = m.Genre.Name,
                Poster = m.Poster,
                Rate = m.Rate,
                Storyline = m.Storyline,
                Title = m.Title,
                Year = m.Year
            })
            .ToListAsync();

            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if(dto.Poster == null)
                return BadRequest("Poster is required!");

            if(!_allowedEx.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            
            if(dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB!");

            var isValid = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if(!isValid)
                return BadRequest("InValid genre ID!");

            using var dataStream = new MemoryStream();

            await dto.Poster.CopyToAsync(dataStream);

            var movie = _mapper.Map<Movie>(dto);
            movie.Poster = dataStream.ToArray();
            // {
            //     GenreId = dto.GenreId,
            //     Title = dto.Title,
            //     Poster = dataStream.ToArray(),
            //     Rate = dto.Rate,
            //     Storyline = dto.Storyline,
            //     Year = dto.Year
            // };

            await _context.AddAsync(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);

            if(movie == null)
                return NotFound($"No movie was found with ID: {id}");

            var isValid = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if(!isValid)
                return BadRequest("InValid genre ID!");

            if(dto.Poster != null)
            {
                if(!_allowedEx.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed!");
            
                if(dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB!");

                using var dataStream = new MemoryStream();

                await dto.Poster.CopyToAsync(dataStream);

                movie.Poster = dataStream.ToArray();
            }
            
            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Storyline = dto.Storyline;
            movie.Rate = dto.Rate;

            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if(movie == null)
                return NotFound($"No movie was found with ID: {id}");
            
            _context.Remove(movie);
            _context.SaveChanges();

            return Ok(movie);
        }
    }
}