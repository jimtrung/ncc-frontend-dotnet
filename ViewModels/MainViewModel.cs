using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand SignUpCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ICommand SettingsCommand { get; private set; }

        public MainViewModel()
        {
            SignUpCommand = new RelayCommand(ExecuteSignUp);
            SignInCommand = new RelayCommand(ExecuteSignIn);
            SettingsCommand = new RelayCommand(ExecuteSettings);
        }

        private void ExecuteSignUp(object parameter)
        {
            MessageBox.Show("Open Sign Up Window.", "Action");
        }

        private void ExecuteSignIn(object parameter)
        {
            MessageBox.Show("Open Sign In Window.", "Action");
        }

        private void ExecuteSettings(object parameter)
        {
            MessageBox.Show("Open Settings Window.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
