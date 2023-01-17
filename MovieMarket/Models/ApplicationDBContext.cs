using Microsoft.EntityFrameworkCore;
using MovieStore.Models;

namespace MovieMarket.Models
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }

    }
}
