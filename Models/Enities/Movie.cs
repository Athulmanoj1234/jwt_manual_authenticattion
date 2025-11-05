namespace jwtmanualauthentication.Models.Enities
{
    public class Movie
    {
        public int MovieId { get; set; }
        public required string MovieName { get; set; }
        public List<ActorMovie>? ActorMovies { get; set; }
    }
}
