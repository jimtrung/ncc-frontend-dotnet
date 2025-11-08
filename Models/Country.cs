using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class Country : INotifyPropertyChanged
    {
        private string _code;
        private string _name;
        private string _iso3;
        private string _phoneCode;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public string Code
        {
            get => _code;
            set { _code = value; OnPropertyChanged(nameof(Code)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Iso3
        {
            get => _iso3;
            set { _iso3 = value; OnPropertyChanged(nameof(Iso3)); }
        }

        public string PhoneCode
        {
            get => _phoneCode;
            set { _phoneCode = value; OnPropertyChanged(nameof(PhoneCode)); }
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
