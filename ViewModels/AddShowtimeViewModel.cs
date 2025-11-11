using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class AddShowtimeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _movieList;
        public ObservableCollection<string> MovieList
        {
            get => _movieList;
            set { _movieList = value; OnPropertyChanged(); }
        }

        private string _selectedMovie;
        public string SelectedMovie
        {
            get => _selectedMovie;
            set 
            { 
                _selectedMovie = value; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedMovieDisplay));
            }
        }
        
        public string SelectedMovieDisplay
        {
            get => $"Bạn đã chọn: {SelectedMovie}";
        }

        public ICommand ButtonCommand { get; private set; }

        public AddShowtimeViewModel()
        {
            MovieList = new ObservableCollection<string>();
            SelectedMovie = string.Empty;

            ButtonCommand = new RelayCommand(ExecuteButton);
        }

        private void ExecuteButton(object parameter)
        {
            string status = string.IsNullOrWhiteSpace(SelectedMovie) 
                ? "Button clicked. No movie currently selected." 
                : $"Button clicked. Selected movie: {SelectedMovie}.";
            
            MessageBox.Show(status, "Button Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
