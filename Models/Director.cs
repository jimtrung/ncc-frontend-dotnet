using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Director : INotifyPropertyChanged
    {
        private Guid _id;
        private string _firstName;
        private string _lastName;
        private DateTime? _dob;
        private int? _age;
        private Gender _gender;
        private string _countryCode;
        private Country _country;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }
        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public string FullName => $"{FirstName} {LastName}";
        public DateTime? Dob
        {
            get => _dob;
            set { _dob = value; OnPropertyChanged(nameof(Dob)); }
        }
        public int? Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }
        public Gender Gender
        {
            get => _gender;
            set { _gender = value; OnPropertyChanged(nameof(Gender)); }
        }
        public string CountryCode
        {
            get => _countryCode;
            set { _countryCode = value; OnPropertyChanged(nameof(CountryCode)); }
        }
        public Country Country
        {
            get => _country;
            set { _country = value; OnPropertyChanged(nameof(Country)); }
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
