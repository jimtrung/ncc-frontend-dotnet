using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class AddMovieController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private ActorService _actorService;
        private DirectorService _directorService;
        private MovieActorService _movieActorService;
        private MovieListController _movieListController;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthTokenUtil(AuthTokenUtil util) => _authTokenUtil = util;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetActorService(ActorService service) => _actorService = service;
        public void SetDirectorService(DirectorService service) => _directorService = service;
        public void SetMovieActorService(MovieActorService service) => _movieActorService = service;
        public void SetMovieListController(MovieListController controller) => _movieListController = controller;

        public TextBox MovieNameField;
        public TextBox MovieDescriptionField;
        public TextBox MovieLanguageField;
        public TextBox MovieDurationField;
        public TextBox MovieRatedField;
        public ListBox DirectorListView;
        public ListBox ActorListView;
        public ListBox GenreListView;
        public TextBox SearchDirectorField;
        public TextBox SearchActorField;
        public TextBox SearchGenreField;

        private ObservableCollection<Director> Directors = new();
        private ObservableCollection<Actor> Actors = new();
        private ObservableCollection<string> Genres = new();

        public void HandleOnOpen()
        {
            // Genres
            var allGenres = Enum.GetNames(typeof(MovieGenre));
            Genres = new ObservableCollection<string>(allGenres);
            if (GenreListView != null)
                GenreListView.ItemsSource = Genres;

            // Directors
            var allDirectors = _directorService.GetAllDirectors() as List<Director>;
            if (allDirectors != null)
            {
                Directors = new ObservableCollection<Director>(allDirectors);
                if (DirectorListView != null)
                    DirectorListView.ItemsSource = Directors;
            }

            // Actors
            var allActors = _actorService.GetAllActors() as List<Actor>;
            if (allActors != null)
            {
                Actors = new ObservableCollection<Actor>(allActors);
                if (ActorListView != null)
                    ActorListView.ItemsSource = Actors;
            }
        }

        public void HandleAddMovieButtonClick()
        {
            if (string.IsNullOrWhiteSpace(MovieNameField.Text) ||
                string.IsNullOrWhiteSpace(MovieDurationField.Text) ||
                string.IsNullOrWhiteSpace(MovieLanguageField.Text) ||
                string.IsNullOrWhiteSpace(MovieRatedField.Text))
            {
                MessageBox.Show("Please enter complete information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Name = MovieNameField.Text.Trim(),
                Description = MovieDescriptionField.Text.Trim(),
                Language = MovieLanguageField.Text.Trim(),
                Premiere = DateTime.Now,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            if (!int.TryParse(MovieDurationField.Text.Trim(), out var duration))
            {
                MessageBox.Show("Duration must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            movie.Duration = duration;

            if (!int.TryParse(MovieRatedField.Text.Trim(), out var rated))
            {
                MessageBox.Show("Rated must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            movie.Rated = rated;

            var selectedGenres = GenreListView.SelectedItems.Cast<MovieGenre>().ToList();
            movie.Genres = selectedGenres;

            var selectedDirector = DirectorListView.SelectedItem as Director;
            if (selectedDirector != null)
                movie.DirectorId = selectedDirector.Id;

            _movieService.InsertMovie(movie);

            var selectedActors = ActorListView.SelectedItems.Cast<Actor>().ToList();
            var actorIds = selectedActors.ConvertAll(a => a.Id);
            _movieActorService.InsertMovieActors(movie.Id, actorIds);

            _movieListController.RefreshData();
            _screenController.NavigateTo<MovieList>();
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<MovieList>();
        }

        public void BindUIControls(TextBox name, TextBox desc, TextBox language, TextBox duration, TextBox rated, ListBox directors, ListBox actors, ListBox genres, TextBox searchDirector, TextBox searchActor, TextBox searchGenre, Button back, Button add)
        {
            MovieNameField = name;
            MovieDescriptionField = desc;
            MovieLanguageField = language;
            MovieDurationField = duration;
            MovieRatedField = rated;
            DirectorListView = directors;
            ActorListView = actors;
            GenreListView = genres;
            SearchDirectorField = searchDirector;
            SearchActorField = searchActor;
            SearchGenreField = searchGenre;

            back.Click += (s, e) => HandleBackButton();
            add.Click += (s, e) => HandleAddMovieButtonClick();
        }
    }
}
