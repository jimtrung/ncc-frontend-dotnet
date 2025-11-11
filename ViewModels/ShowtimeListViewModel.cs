using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class ShowtimeListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _showtimeList;
        public ObservableCollection<string> ShowtimeList
        {
            get => _showtimeList;
            set { _showtimeList = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand DeleteAllCommand { get; private set; }
        public ICommand AddShowtimeCommand { get; private set; }

        public ShowtimeListViewModel()
        {
            ShowtimeList = new ObservableCollection<string>();

            CloseCommand = new RelayCommand(ExecuteClose);
            DeleteAllCommand = new RelayCommand(ExecuteDeleteAll);
            AddShowtimeCommand = new RelayCommand(ExecuteAddShowtime);
        }

        private void ExecuteClose(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteDeleteAll(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete ALL showtimes? This cannot be undone.", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                ShowtimeList.Clear();
                MessageBox.Show("All showtimes deleted.", "Operation Complete");
            }
        }

        private void ExecuteAddShowtime(object parameter)
        {
            MessageBox.Show("Open Add Showtime Window logic goes here.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
