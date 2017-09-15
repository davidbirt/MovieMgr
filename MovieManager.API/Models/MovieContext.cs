using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MovieManager.API.Models
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
                        SubGenres = x.SubGenres.ToList().Select(n => new Genre() {Id=n.Id, Name = n.Name }),
                        Id = x.Id,
                        DateReleased = x.ReleaseDate,
                        Description = x.Description
                    });
            }
        }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<SubGenre> SubGenres { get; set; }

        public MovieDTO GetMovie(int id)
        {
            try
            {
                return MoviesDTO.First(mv => mv.Id == id);
            }
            catch (Exception)
            {
                // todo : Log off the error
                return null;
            }
        }
    }

    
}