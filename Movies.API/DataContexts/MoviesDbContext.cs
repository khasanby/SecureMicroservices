using Microsoft.EntityFrameworkCore;
using Movies.API.Entities;

namespace Movies.API.DataContexts
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Represents the Movies table in the database.
        /// </summary>
        public DbSet<Movie> Movies { get; set; }
    }
}