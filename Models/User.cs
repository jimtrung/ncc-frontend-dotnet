using System.ComponentModel;

namespace Theater_Management_FE.Models
{
    public class User : INotifyPropertyChanged
    {
        private Guid _id;
        private string _username;
        private string _email;
        private string _phoneNumber;
        private string _password;
        private UserRole _role;
        private Provider _provider;
        private string? _token;
        private string? _otp;
        private bool _verified;
        private DateTime _createdAt;
        private DateTime _updatedAt;
        public Guid Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }
        public UserRole Role
        {
            get => _role;
            set { _role = value; OnPropertyChanged(nameof(Role)); }
        }
        public Provider Provider
        {
            get => _provider;
            set { _provider = value; OnPropertyChanged(nameof(Provider)); }
        }
        public string Token
        {
            get => _token;
            set { _token = value; OnPropertyChanged(nameof(Token)); }
        }
        public string OTP
        {
            get => _otp;
            set { _otp = value; OnPropertyChanged(nameof(OTP)); }
        }
        public bool Verified
        {
            get => _verified;
            set { _verified = value; OnPropertyChanged(nameof(Verified)); }
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
