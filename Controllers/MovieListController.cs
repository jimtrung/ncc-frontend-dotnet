using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class MovieListController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private ActorService _actorService;
        private DirectorService _directorService;
        private MovieActorService _movieActorService;
        private Guid _uuid;

        public ObservableCollection<Movie> MovieList { get; set; } = new ObservableCollection<Movie>();

        public DataGrid movieTable;
        public Button closeBtn;
        public Button addMovieBtn;
        public Button deleteAllBtn;

        public DataGridTextColumn nameColumn;
        public DataGridTextColumn directorColumn;
        public DataGridTextColumn genresColumn;
        public DataGridTextColumn premiereColumn;
        public DataGridTextColumn durationColumn;
        public DataGridTextColumn ratedColumn;
        public DataGridTextColumn languageColumn;

        public void SetScreenController(ScreenController screenController) => _screenController = screenController;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetMovieService(MovieService movieService) => _movieService = movieService;
        public void SetActorService(ActorService actorService) => _actorService = actorService;
        public void SetDirectorService(DirectorService directorService) => _directorService = directorService;
        public void SetMovieActorService(MovieActorService movieActorService) => _movieActorService = movieActorService;

        public void HandleOnOpen()
        {
            if (movieTable == null) return;

            nameColumn.Binding = new System.Windows.Data.Binding("Name");
            directorColumn.Binding = new System.Windows.Data.Binding("DirectorId") { Converter = new GuidToStringConverter() };
            genresColumn.Binding = new System.Windows.Data.Binding("Genres") { Converter = new ListToStringConverter() };
            premiereColumn.Binding = new System.Windows.Data.Binding("Premiere") { Converter = new DateToStringConverter("dd/MM/yyyy") };
            durationColumn.Binding = new System.Windows.Data.Binding("Duration");
            ratedColumn.Binding = new System.Windows.Data.Binding("Rated");
            languageColumn.Binding = new System.Windows.Data.Binding("Language");

            movieTable.ItemsSource = MovieList;

            movieTable.SelectionChanged += (s, e) =>
            {
                if (movieTable.SelectedItem is Movie m)
                {
                    _uuid = m.Id;
                    HandleClickItem(_uuid);
                }
            };

            RefreshData();
        }

        public void HandleAddMovie()
        {
            _screenController.NavigateTo<AddMovie>();
        }

        public void HandleClickItem(Guid id)
        {
            try
            {
                var movieInfoWindow = new MovieInformation();
                var controller = movieInfoWindow.DataContext as MovieInformationController;
                controller.SetScreenController(_screenController);
                controller.SetMovieService(_movieService);
                controller.SetAuthTokenUtil(_authTokenUtil);
                controller.SetMovieListController(this);
                controller.SetMovieId(id);
                movieInfoWindow.ShowDialog();
            }
            catch (Exception) { }
        }

        public void HandleDeleteAllMovie()
        {
            var result = MessageBox.Show("Are you sure you want to delete all movies?", "Delete confirmation", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                _movieService.DeleteAllMovies();
                RefreshData();
            }
        }

        public void HandleCloseBtn() => _screenController.NavigateTo<HomePageManager>();

        public void RefreshData()
        {
            if (_movieService != null)
            {
                var movies = _movieService.GetAllMovies();
                MovieList.Clear();
                foreach (var m in movies)
                    MovieList.Add(m);
            }
        }

        public void UpdateMovie(Movie updatedMovie)
        {
            var existing = MovieList.FirstOrDefault(m => m.Id == updatedMovie.Id);
            if (existing != null)
            {
                int idx = MovieList.IndexOf(existing);
                MovieList[idx] = updatedMovie;
            }
        }
    }
}
