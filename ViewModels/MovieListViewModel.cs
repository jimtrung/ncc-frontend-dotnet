using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class MovieListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _movieList;
        public ObservableCollection<string> MovieList
        {
            get => _movieList;
            set { _movieList = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand DeleteAllCommand { get; private set; }
        public ICommand AddMovieCommand { get; private set; }

        public MovieListViewModel()
        {
            MovieList = new ObservableCollection<string>();

            CloseCommand = new RelayCommand(ExecuteClose);
            DeleteAllCommand = new RelayCommand(ExecuteDeleteAll);
            AddMovieCommand = new RelayCommand(ExecuteAddMovie);
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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete ALL movies? This cannot be undone.", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MovieList.Clear();
                MessageBox.Show("All movies deleted.", "Operation Complete");
            }
        }

        private void ExecuteAddMovie(object parameter)
        {
            MessageBox.Show("Open Add Movie Window logic goes here.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
