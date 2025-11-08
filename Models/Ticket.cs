using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Ticket : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _userId;
        private User _user;
        private Guid _showtimeId;
        private Showtime _showtime;
        private Guid _seatId;
        private Seat _seat;
        private int _price;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public Guid UserId
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(nameof(UserId)); }
        }
        public User User
        {
            get => _user;
            set { _user = value; OnPropertyChanged(nameof(User)); }
        }
        public Guid ShowtimeId
        {
            get => _showtimeId;
            set { _showtimeId = value; OnPropertyChanged(nameof(ShowtimeId)); }
        }
        public Showtime Showtime
        {
            get => _showtime;
            set { _showtime = value; OnPropertyChanged(nameof(Showtime)); }
        }
        public Guid SeatId
        {
            get => _seatId;
            set { _seatId = value; OnPropertyChanged(nameof(SeatId)); }
        }
        public Seat Seat
        {
            get => _seat;
            set { _seat = value; OnPropertyChanged(nameof(Seat)); }
        }
        public int Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
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
