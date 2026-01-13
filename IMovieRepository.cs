using jwtmanualauthentication.Models.Enities;

namespace jwtmanualauthentication
{
    public interface IMovieRepository
    {
        Task<Movie> getMovieById(int id);
    }
}
