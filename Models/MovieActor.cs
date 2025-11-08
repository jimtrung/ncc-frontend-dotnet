using System.ComponentModel;
namespace Theater_Management_FE.Models
{
    public class MovieActor : INotifyPropertyChanged
    {
        private Guid _movieId;
        private Movie _movie;
        private Guid _actorId;
        private Actor _actor;
        public Guid MovieId
        {
            get => _movieId;
            set { _movieId = value; OnPropertyChanged(nameof(MovieId)); }
        }
        public Movie Movie
        {
            get => _movie;
            set { _movie = value; OnPropertyChanged(nameof(Movie)); }
        }
        public Guid ActorId
        {
            get => _actorId;
            set { _actorId = value; OnPropertyChanged(nameof(ActorId)); }
        }
        public Actor Actor
        {
            get => _actor;
            set { _actor = value; OnPropertyChanged(nameof(Actor)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
