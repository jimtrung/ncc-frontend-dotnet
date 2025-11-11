using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand SignUpCommand { get; private set; }

        public SignUpViewModel()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            SignUpCommand = new RelayCommand(ExecuteSignUp);
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteSignUp(object parameter)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please fill in all required fields (Username, Email, Password).", "Sign Up Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Attempting to register user: {Username}", "Sign Up Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
