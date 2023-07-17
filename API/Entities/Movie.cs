
namespace API.Entities
{
    public class Movie
    {
        // if i make this first prop id entityFramework it will be aware know this is primary key
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }
        [MaxLength(2500)]
        public string Storyline { get; set; }
        public byte[] Poster { get; set; }
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}