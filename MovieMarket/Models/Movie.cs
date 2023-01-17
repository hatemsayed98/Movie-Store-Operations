using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models
{
    public class Movie
    {
        //EF knows it is Id, and auto-Increment as it's int type
        public int Id { get; set; }
        [Required, MaxLength(200)]
        public string Name { get; set; } 
        public int Year { get; set; } //Required by default because of int
        public double Rate { get; set; }
        [Required, MaxLength(2500)]
        public string StoryLine { get; set; }
        [Required]
        public byte [] Poster { get; set; }
        //EF will know that GenreId is FK
        public byte GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
