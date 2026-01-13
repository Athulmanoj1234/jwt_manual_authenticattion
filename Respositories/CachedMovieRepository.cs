using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models.Enities;
using Microsoft.Extensions.Caching.Memory;

namespace jwtmanualauthentication.Respositories
{
    public class CachedMovieRepository : IMovieRepository
    {
        public MovieRepository _decorated;
        public ApplicationDbContext _dbcontext;
        public IMemoryCache _memoryCache;
        public CachedMovieRepository(ApplicationDbContext dbContext, MovieRepository decorated, IMemoryCache memoryCache) {
            this._decorated = decorated;
            this._dbcontext = dbContext;
            this._memoryCache = memoryCache;
        }

        public async Task<Movie> getMovieById(int id) {
            string key = $"movie-{id}";
            return await this._memoryCache.GetOrCreateAsync(key, async entry => {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                return await this._decorated.getMovieById(id);
            });
        }
    }
}
