using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieManager.Models
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base("MoviesDB")
        {
            //Database.SetInitializer<MovieContext>(new DropCreateDatabaseIfModelChanges<MovieContext>());;
        }

        public DbSet<Movie> Movies { get; set; }
        public IEnumerable<MovieDTO> MoviesDTO
        {
            get
            {
                return Movies.Include(mbox => mbox.Director)
                    .Include(m => m.MainGenre)
                    .Include(m => m.SubGenres).ToList()
                    .Select(x => new MovieDTO()
                    {
                        DirectorName = x.Director.Name,
                        Length = x.Length,
                        MainGenreName = x.MainGenre.Name,
                        Title = x.Title,
                        SubGenres = x.SubGenres.ToList().Select(n => n.Name)
                    });
            }
        }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SubGenre> SubGenres { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    
}