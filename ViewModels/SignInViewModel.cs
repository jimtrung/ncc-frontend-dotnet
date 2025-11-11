using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;
using System;

namespace Theater_Management_FE.ViewModels
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ICommand ForgotPasswordCommand { get; private set; }

        public SignInViewModel()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            SignInCommand = new RelayCommand(ExecuteSignIn);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteSignIn(object parameter)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please enter both username and password.", "Sign In Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show($"Attempting to sign in with Username: {Username}", "Sign In Action");
        }

        private void ExecuteForgotPassword(object parameter)
        {
            MessageBox.Show("Open Forgot Password recovery process.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
