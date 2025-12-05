
namespace Theater_Management_FE.Models
{
    public class MovieActor
    {
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public Guid ActorId { get; set; }
        public Actor Actor { get; set; }
    }
}
