using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MovieManager.API.Models
{
    public class MovieDTO
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string DirectorName { get; set; }
        public string MainGenreName { get; set; }
        public int Length { get; set; }
        public IEnumerable<Genre> SubGenres { get; set; }
        public DateTime DateReleased { get; set; }
        public string Description { get; set; }
    }

    public class Movie
    {
        public Movie()
        {
            this.SubGenres = new HashSet<SubGenre>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        
        public Genre MainGenre { get; set; }
        public virtual ICollection<SubGenre> SubGenres { get; set; }
        [Required]
        public virtual Director Director { get; set;}
        [Required]
        public DateTime ReleaseDate { get; set; }
        /// <summary>
        /// Length of the movie in Minutes
        /// </summary>
        [Required]
        public int Length { get; set; }
        [Required]
        public string Description { get; set; }
    }

    public class Director
    {
        public Director()
        {
            Movies = new HashSet<Movie>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Movie> Movies { get; set; }
    }

    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SubGenre : Genre
    {
        public SubGenre()
        {
            this.Movies = new HashSet<Movie>();
        }
        public virtual ICollection<Movie> Movies { get; set; }
    }
}