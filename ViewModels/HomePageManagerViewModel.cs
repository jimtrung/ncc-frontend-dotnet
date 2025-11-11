using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;
using System;

namespace Theater_Management_FE.ViewModels
{
    public class HomePageManagerViewModel : INotifyPropertyChanged
    {
        public ICommand MovieCommand { get; private set; }
        public ICommand AuditoriumCommand { get; private set; }
        public ICommand ShowtimeCommand { get; private set; }
        public ICommand ProfileCommand { get; private set; }
        public ICommand LogOutCommand { get; private set; }

        public HomePageManagerViewModel()
        {
            MovieCommand = new RelayCommand(ExecuteMovie);
            AuditoriumCommand = new RelayCommand(ExecuteAuditorium);
            ShowtimeCommand = new RelayCommand(ExecuteShowtime);
            ProfileCommand = new RelayCommand(ExecuteProfile);
            LogOutCommand = new RelayCommand(ExecuteLogOut);
        }

        private void ExecuteMovie(object parameter)
        {
            MessageBox.Show("Navigate to Movie Management view.", "Navigation");
        }

        private void ExecuteAuditorium(object parameter)
        {
            MessageBox.Show("Navigate to Auditorium Management view.", "Navigation");
        }

        private void ExecuteShowtime(object parameter)
        {
            MessageBox.Show("Navigate to Showtime Management view.", "Navigation");
        }

        private void ExecuteProfile(object parameter)
        {
            MessageBox.Show("Navigate to Profile view.", "Navigation");
        }

        private void ExecuteLogOut(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Confirm Log Out", MessageBoxButton.YesNo);
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
