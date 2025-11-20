using System.Windows.Controls;
using Theater_Management_FE.Controllers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class MovieInformationController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private MovieListController _movieListController;
        private Guid _movieId;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetMovieListController(MovieListController controller) => _movieListController = controller;
        public void SetMovieId(Guid id) => _movieId = id;

        public TextBox MovieNameField;
        public TextBox MovieDescriptionField;
        public TextBox MovieDirectorIdField;
        public TextBox MovieGenresField;
        public TextBox MoviePremiereField;
        public TextBox MovieDurationField;
        public TextBox MovieLanguageField;
        public TextBox MovieRatedField;

        public Button BackButton;
        public Button EditButton;
        public Button DeleteButton;

        public void HandleOnOpen()
        {
            var movie = _movieService.GetMovieById(_movieId);
            if (movie == null) return;

            MovieNameField.Text = movie.Name;
            MovieDescriptionField.Text = movie.Description;
            MovieLanguageField.Text = movie.Language;
            MovieDurationField.Text = movie.Duration.ToString();
            MovieRatedField.Text = movie.Rated.ToString();
            MovieDirectorIdField.Text = movie.DirectorId?.ToString() ?? "";
            MoviePremiereField.Text = movie.Premiere?.ToString() ?? "";
            MovieGenresField.Text = string.Join(", ", movie.Genres);
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<MovieList>();
        }

        public void HandleEditButton()
        {
            var updatedMovie = new Movie
            {
                Name = MovieNameField.Text.Trim(),
                Description = MovieDescriptionField.Text.Trim(),
                Language = MovieLanguageField.Text.Trim()
            };

            if (Guid.TryParse(MovieDirectorIdField.Text.Trim(), out var directorId))
                updatedMovie.DirectorId = directorId;

            if (int.TryParse(MovieRatedField.Text.Trim(), out var rated))
                updatedMovie.Rated = rated;

            if (int.TryParse(MovieDurationField.Text.Trim(), out var duration))
                updatedMovie.Duration = duration;

            if (DateTime.TryParse(MoviePremiereField.Text.Trim(), out var premiere))
                updatedMovie.Premiere = premiere;

            var genresText = MovieGenresField.Text.Trim();
            if (!string.IsNullOrEmpty(genresText))
            {
                var genreStrings = genresText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(s => s.Trim());

                var genres = new List<MovieGenre>();
                foreach (var g in genreStrings)
                {
                    if (Enum.TryParse<MovieGenre>(g, true, out var genre))
                        genres.Add(genre);
                }

                updatedMovie.Genres = genres;
            }

            _movieService.UpdateMovie(_movieId, updatedMovie);
            var movie = _movieService.GetMovieById(_movieId);
            _movieListController.UpdateMovie(movie);
            _screenController.NavigateTo<MovieList>();
        }

        public void HandleDeleteButton()
        {
            _movieService.DeleteMovieById(_movieId);
            _movieListController.RefreshData();
            _screenController.NavigateTo<MovieList>();
        }

        public void BindUIControls(TextBox name, TextBox desc, TextBox directorId, TextBox genres, TextBox premiere, TextBox duration, TextBox language, TextBox rated, Button back, Button edit, Button delete)
        {
            MovieNameField = name;
            MovieDescriptionField = desc;
            MovieDirectorIdField = directorId;
            MovieGenresField = genres;
            MoviePremiereField = premiere;
            MovieDurationField = duration;
            MovieLanguageField = language;
            MovieRatedField = rated;
            BackButton = back;
            EditButton = edit;
            DeleteButton = delete;

            BackButton.Click += (s, e) => HandleBackButton();
            EditButton.Click += (s, e) => HandleEditButton();
            DeleteButton.Click += (s, e) => HandleDeleteButton();
        }
    }
}
