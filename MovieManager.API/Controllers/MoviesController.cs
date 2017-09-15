using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MovieManager.Models;
using AutoMapper;

namespace MovieManager.Controllers
{
    public class MoviesController : ApiController
    {
        IMapper _mapper;
        public MoviesController(IMapper mapper)
        {
            _mapper = mapper;
        }
        private MovieContext db = new MovieContext();

        // GET: api/Movies
        public IHttpActionResult GetMovies()
        {
            var movies = db.MoviesDTO;
            return Ok(movies);
        }
   
        // GET: api/Movies/5
        public IHttpActionResult GetMovie(int id)
        {
            MovieDTO movie = db.GetMovie(id); //await db.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/Movies
        public async Task<IHttpActionResult> PostMovie(MovieDTO movie)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Movie instance = db.Movies.First(mv => mv.Id == movie.Id);
                instance = _mapper.Map<MovieDTO, Movie>(movie, instance);
                int results = await db.SaveChangesAsync();
                return Ok($"{results} records affected.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int id)
        {
            return db.Movies.Count(e => e.Id == id) > 0;
        }
    }
}