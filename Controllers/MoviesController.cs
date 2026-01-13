using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models;
using jwtmanualauthentication.Models.Enities;
using jwtmanualauthentication.Respositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace jwtmanualauthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        public ApplicationDbContext _dbContext;
        public IMovieRepository _movieRepository;

        public MoviesController(ApplicationDbContext DbContext, IMovieRepository movieRepository)
        {
            this._dbContext = DbContext;
            this._movieRepository = movieRepository;
        }

        [HttpPost("AddActors")]
        public async Task<IActionResult> CreateActors(ActorDto Actor)
        {
            try
            {
                var actor = new Actor()
                {
                    ActorId = Actor.Id,
                    ActorName = Actor.ActorName,
                };
                this._dbContext.Actors.Add(actor); //dbcontext.Add is a synchronous operation because this method only adds data to method only adds the entity to the in-memory change tracker of the DbContext,
                await this._dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddMovies")]
        public async Task<IActionResult> CreateMovies(MoviesDto Movie)
        {
            try
            {
                var movie = new Movie()
                {
                    MovieId = Movie.Id,
                    MovieName = Movie.MovieName,
                };
                this._dbContext.Movies.Add(movie); //dbcontext.Add is a synchronous operation because this method only adds data to method only adds the entity to the in-memory change tracker of the DbContext,
                await this._dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddActorMovie")]
        public async Task<IActionResult> CreateActorMovie(ActorMovieDto ActorMovie) {
            try
            {
                var actorMovie = new ActorMovie()
                {
                    Id = ActorMovie.Id,
                    MovieId = ActorMovie.MovieId,
                    ActorId = ActorMovie.ActorId,
                };
                this._dbContext.ActorMovies.Add(actorMovie); //dbcontext.Add is a synchronous operation because this method only adds data to method only adds the entity to the in-memory change tracker of the DbContext,
                await this._dbContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetActorMovies")]
        public async Task<IActionResult> GetActorMovie() {
            try
            {
                var actorMovie = await this._dbContext.ActorMovies
                    .Include(am => am.Actor)
                    .Include(am => am.Movie)
                    .ToListAsync();
                return Ok(actorMovie);
            }
            catch (Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getMovieByid/{id:int}")]
        
        public async Task<IActionResult> GetMoviebyId(int id)
        {
            var result = await this._movieRepository.getMovieById(id);
            return Ok(result);
        }

    }
}
