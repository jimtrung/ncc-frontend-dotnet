using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
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

        private string _passwordPlaceholder;
        public string PasswordPlaceholder
        {
            get => _passwordPlaceholder;
            set { _passwordPlaceholder = value; OnPropertyChanged(); }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        private string _verified;
        public string Verified
        {
            get => _verified;
            set { _verified = value; OnPropertyChanged(); }
        }

        private string _createdDate;
        public string CreatedDate
        {
            get => _createdDate;
            set { _createdDate = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand EditProfileCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public ProfileViewModel()
        {
            Username = "user_theater_001";
            Email = "user.theater@example.com";
            PasswordPlaceholder = "********";
            PhoneNumber = "0123456789";
            Verified = "Yes";
            CreatedDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd HH:mm:ss");

            BackCommand = new RelayCommand(ExecuteBack);
            EditProfileCommand = new RelayCommand(ExecuteEditProfile);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteEditProfile(object parameter)
        {
            MessageBox.Show("Open Edit Profile Window for user: " + Username, "Action");
        }

        private void ExecuteLogOut(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (parameter is Window window)
                {
                    window.Close();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
