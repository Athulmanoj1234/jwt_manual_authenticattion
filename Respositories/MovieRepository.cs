using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace jwtmanualauthentication.Respositories
{
    public class MovieRepository : IMovieRepository
    {
        public ApplicationDbContext _dbContext { get; set; }
        public MovieRepository(ApplicationDbContext dbContext) {
            this._dbContext = dbContext;
        }

        public async Task<Movie> getMovieById(int id) {
            Movie movie = await this._dbContext.Movies.FirstOrDefaultAsync(movie => movie.MovieId == id);
            return movie;
        }
    }
}
