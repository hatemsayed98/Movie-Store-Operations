using MovieStore.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MovieStore.ViewModel
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        public int Year { get; set; }
        [Range(1, 10)]
        public double Rate { get; set; }
        [Required, StringLength(2500)]
        public string StoryLine { get; set; }

        [DisplayName("Movie Poster")]
        public byte[]? Poster { get; set; }

        [DisplayName("Genre")]
        public byte GenreId { get; set; }
        public IEnumerable<Genre>? Genres { get; set; }
    }
}
