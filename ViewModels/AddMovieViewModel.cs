using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class AddMovieViewModel : INotifyPropertyChanged
    {
        private string _movieName;
        private string _movieDescription;
        private string _movieRated;
        private string _movieDuration;
        private string _movieLanguage;
        private string _searchGenreText;
        private string _searchDirectorText;
        private string _searchActorText;
        private string _selectedDirector;

        public string MovieName
        {
            get => _movieName;
            set
            {
                _movieName = value;
                OnPropertyChanged();
                ((RelayCommand)AddMovieCommand).RaiseCanExecuteChanged();
            }
        }
        
        public string MovieDescription
        {
            get => _movieDescription;
            set { _movieDescription = value; OnPropertyChanged(); }
        }

        public string MovieRated
        {
            get => _movieRated;
            set
            {
                _movieRated = value;
                OnPropertyChanged();
                ((RelayCommand)AddMovieCommand).RaiseCanExecuteChanged();
            }
        }

        public string MovieDuration
        {
            get => _movieDuration;
            set
            {
                _movieDuration = value;
                OnPropertyChanged();
                ((RelayCommand)AddMovieCommand).RaiseCanExecuteChanged();
            }
        }

        public string MovieLanguage
        {
            get => _movieLanguage;
            set { _movieLanguage = value; OnPropertyChanged(); }
        }

        public string SearchGenreText
        {
            get => _searchGenreText;
            set
            {
                _searchGenreText = value;
                OnPropertyChanged();
                FilterGenres();
            }
        }

        public string SearchDirectorText
        {
            get => _searchDirectorText;
            set
            {
                _searchDirectorText = value;
                OnPropertyChanged();
                FilterDirectors();
            }
        }

        public string SearchActorText
        {
            get => _searchActorText;
            set
            {
                _searchActorText = value;
                OnPropertyChanged();
                FilterActors();
            }
        }

        public ObservableCollection<string> AllGenres { get; set; }
        public ObservableCollection<string> FilteredGenres { get; set; }

        public ObservableCollection<string> AllDirectors { get; set; }
        public ObservableCollection<string> FilteredDirectors { get; set; }

        public string SelectedDirector
        {
            get => _selectedDirector;
            set
            {
                _selectedDirector = value;
                OnPropertyChanged();
                ((RelayCommand)AddMovieCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<string> AllActors { get; set; }
        public ObservableCollection<string> FilteredActors { get; set; }

        public ICommand BackCommand { get; private set; }
        public ICommand AddMovieCommand { get; private set; }

        public AddMovieViewModel()
        {
            InitializeCollections();

            BackCommand = new RelayCommand(ExecuteBack);
            AddMovieCommand = new RelayCommand(ExecuteAddMovie, CanExecuteAddMovie);
        }

        private void InitializeCollections()
        {
            AllGenres = new ObservableCollection<string> { "Action", "Comedy", "Drama", "Horror", "Sci-Fi" };
            FilteredGenres = new ObservableCollection<string>(AllGenres);

            AllDirectors = new ObservableCollection<string> { "Christopher Nolan", "Denis Villeneuve", "Greta Gerwig", "Steven Spielberg" };
            FilteredDirectors = new ObservableCollection<string>(AllDirectors);

            AllActors = new ObservableCollection<string> { "Tom Cruise", "Zendaya", "Leonardo DiCaprio", "Margot Robbie", "Ryan Gosling" };
            FilteredActors = new ObservableCollection<string>(AllActors);
        }

        private void FilterGenres()
        {
            if (AllGenres == null) return; 

            FilteredGenres.Clear();
            var filteredList = AllGenres
                .Where(g => g.IndexOf(SearchGenreText ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0 || string.IsNullOrWhiteSpace(SearchGenreText));
            foreach (var genre in filteredList)
            {
                FilteredGenres.Add(genre);
            }
        }

        private void FilterDirectors()
        {
            if (AllDirectors == null) return;

            FilteredDirectors.Clear();
            var filteredList = AllDirectors
                .Where(d => d.IndexOf(SearchDirectorText ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0 || string.IsNullOrWhiteSpace(SearchDirectorText));
            foreach (var director in filteredList)
            {
                FilteredDirectors.Add(director);
            }
        }

        private void FilterActors()
        {
            if (AllActors == null) return;

            FilteredActors.Clear();
            var filteredList = AllActors
                .Where(a => a.IndexOf(SearchActorText ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0 || string.IsNullOrWhiteSpace(SearchActorText));
            foreach (var actor in filteredList)
            {
                FilteredActors.Add(actor);
            }
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private bool CanExecuteAddMovie(object parameter)
        {
            bool isValidRated = int.TryParse(MovieRated, out _);
            bool isValidDuration = int.TryParse(MovieDuration, out _);

            bool hasSelectedDirector = !string.IsNullOrWhiteSpace(SelectedDirector);

            return !string.IsNullOrWhiteSpace(MovieName)
                && isValidRated
                && isValidDuration
                && hasSelectedDirector;
        }

        private void ExecuteAddMovie(object parameter)
        {
            string directorName = SelectedDirector ?? "None Selected";

            string message = $"Attempting to add Movie:\nName: {MovieName}\nRated: {MovieRated}\nDuration: {MovieDuration}\nLanguage: {MovieLanguage}\n"
                + $"Description: {MovieDescription}\nDirector: {directorName}";

            MessageBox.Show(message, "Movie Data Summary");
            
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
