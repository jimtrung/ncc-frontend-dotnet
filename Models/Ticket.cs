using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Ticket : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _userId;
        private Guid _showtimeId;
        private string _seatName;
        private int _price;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public string Seatname
        {
            get => _seatName;
            set { _seatName = value; OnPropertyChanged(nameof(Seatname)); }
        }
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public Guid Userid
        {
            get => _userId;
            set { _userId = value; OnPropertyChanged(nameof(Userid)); }
        }
        public Guid Showtimeid
        {
            get => _showtimeId;
            set { _showtimeId = value; OnPropertyChanged(nameof(Showtimeid)); }
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
