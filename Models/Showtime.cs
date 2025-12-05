using System;
using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Showtime : INotifyPropertyChanged
    {
        // --- Fields ---
        private Guid _id;
        private Guid _movieId;
        private Guid _auditoriumId;
        private DateTime _startTime;
        private DateTime _endTime;
        private DateTime _showDate;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        private string _movieName;
        private string _auditoriumName;
        private int _quantity;

        // --- Properties ---
        public Guid Id
        {
            get => _id;
            set => SetField(ref _id, value, nameof(Id));
        }

        public Guid MovieId
        {
            get => _movieId;
            set => SetField(ref _movieId, value, nameof(MovieId));
        }

        public Guid AuditoriumId
        {
            get => _auditoriumId;
            set => SetField(ref _auditoriumId, value, nameof(AuditoriumId));
        }

        public DateTime StartTime
        {
            get => _startTime;
            set => SetField(ref _startTime, value, nameof(StartTime));
        }

        public DateTime EndTime
        {
            get => _endTime;
            set => SetField(ref _endTime, value, nameof(EndTime));
        }

        public DateTime ShowDate
        {
            get => _showDate;
            set => SetField(ref _showDate, value, nameof(ShowDate));
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetField(ref _createdAt, value, nameof(CreatedAt));
        }

        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetField(ref _updatedAt, value, nameof(UpdatedAt));
        }

        public string MovieName
        {
            get => _movieName;
            set => SetField(ref _movieName, value, nameof(MovieName));
        }

        public string AuditoriumName
        {
            get => _auditoriumName;
            set => SetField(ref _auditoriumName, value, nameof(AuditoriumName));
        }

        public int Quantity
        {
            get => _quantity;
            set => SetField(ref _quantity, value, nameof(Quantity));
        }

        // --- Readonly string properties for display ---
        public string StartTimeString => StartTime.ToUniversalTime().ToString("HH:mm");
        public string EndTimeString => EndTime.ToUniversalTime().ToString("HH:mm");
        public string ShowDateString => ShowDate.ToString("yyyy-MM-dd");

        // --- INotifyPropertyChanged ---
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // --- Helper method ---
        private void SetField<T>(ref T field, T value, string propertyName)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
