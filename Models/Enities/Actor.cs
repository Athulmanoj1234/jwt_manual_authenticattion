namespace jwtmanualauthentication.Models.Enities
{
    public class Actor
    {
        public int ActorId { get; set; }
        public required string ActorName { get; set; }
        public List<ActorMovie>? ActorMovies { set; get; }
        
    }
}
