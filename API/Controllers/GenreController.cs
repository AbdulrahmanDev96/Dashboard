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
            // hrow7 ala table el genres we ageb meno el name
            // OrderBy : hana ana h3mel response le el data mtrtbah fe list
            // akne 3malt sort leha be el name hwa keda hy3mel order le data 
            // we hya rg3a men database be name bt3ha
            var genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

            return Ok(genres);
        }

        [HttpPost]
        // hb3et gowa el body no3o GenreDto
        public async Task<ActionResult> CreateItem(GenreDto dto)
        {
            // new Syntax Genre genre = new()
            // m7tag any a3mel create New Genre gowa el database 
            // fa hst5dem new Keyword we hgeb el Database Genre table nfsha
            var genre = new Genre {Name = dto.Name};
            // h3mel initialize le value el ana 3yzha

            // h3mel add le no3 el genre el fo2 we howa el name
            // await _context.Genre.AddAsync(genre);
            await _context.AddAsync(genre);
            // 3l4an an2el el object men mogred object fe el memory
            // le (row) gowa database
            _context.SaveChanges();

            // return el object el at3mlow creation
            // m7tag t3mel return le values kolha
            return Ok(genre);
        }

        [HttpPut("{id}")]
        // wel 7aga lazem ab3et el id pta3 el object el 3ayz a3mel yleh update
        // hstkbel el id men front end
        public async Task<ActionResult> UpdateItem(int id, [FromBody] GenreDto dto)
        {
            // m7atg ataked en fe genre fe database be el id dah
            // lw no3ow int hst3mel (findAsync) low no3ow byte hsteml (SingleOrDefaultAsync)
            // SingleOrDefaultAsync ht7tag lamda expression 3l4an y3mel selection
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