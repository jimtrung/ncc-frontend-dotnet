using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Seat : INotifyPropertyChanged
    {
        private Guid _id;
        private Guid _auditoriumId;
        private Auditorium _auditorium;
        private string _row;
        private int _number;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
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
        public string Row
        {
            get => _row;
            set { _row = value; OnPropertyChanged(nameof(Row)); }
        }
        public int Number
        {
            get => _number;
            set { _number = value; OnPropertyChanged(nameof(Number)); }
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
