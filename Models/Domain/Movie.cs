using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStoreMvc.Models.Domain
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? ReleaseYear { get; set; }
        public string? MovieImage { get; set; }
        [Required]
        public string? Cast { get; set; }
        [Required]
        public string? Director { get; set; }
        [NotMapped]
        public  IFormFile? ImageFile { get; set; }
        [NotMapped]
        [Required]
        public List<int>? Genres { get; set; }
        public IEnumerable<SelectListItem>? GenreList;

        public ICollection<MovieGenre>? MovieGenres { get; set; } // Navigation property
        [NotMapped]
        public string? GenreNames { get; set; }    // 
        [NotMapped]
        public MultiSelectList MultiGenreList;

    }

}
