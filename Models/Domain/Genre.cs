using System.ComponentModel.DataAnnotations;

namespace MovieStoreMvc.Models.Domain
{
    public class Genre
    {
        public int ID { get; set; }
        [Required]
        public string? GenreName { get; set; }
        public ICollection<MovieGenre>? MovieGenres { get; set; } // Navigation property
    }
}
