using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Models
{
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }        
    }
}
