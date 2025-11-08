using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Showtime : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _movieId;
        private Movie _movie;
        private Guid _auditoriumId;
        private Auditorium _auditorium;
        private DateTime _startTime;
        private DateTime _endTime;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
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
        public Guid AuditoriumId
        {
            get => _auditoriumId;
            set { _auditoriumId = value; OnPropertyChanged(nameof(AuditoriumId)); }
        }
        public Auditorium Auditorium
        {
            get => _auditorium;
            set { _auditorium = value; OnPropertyChanged(nameof(Auditorium)); }
        }
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }
        public DateTime EndTime
        {
            get => _endTime;
            set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }
        public DateTime CreatedAt
        {
            get => _createdAt;
            set { _createdAt = value; OnPropertyChanged(nameof(CreatedAt)); }
        }
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set { _updatedAt = value; OnPropertyChanged(nameof(UpdatedAt)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
