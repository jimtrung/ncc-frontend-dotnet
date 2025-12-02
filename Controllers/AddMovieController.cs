using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Views;
using System.Windows.Media.Imaging;
using System.IO;

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

        // UI Controls
        public TextBox movieNameField;
        public TextBox movieDescriptionField;
        public TextBox movieLanguageField;
        public TextBox movieDurationField;
        public TextBox movieRatedField;
        
        public ListView genreListView;
        public ListView directorListView;
        public ListView actorListView;
        
        public TextBox searchGenreField;
        public TextBox searchDirectorField;
        public TextBox searchActorField;

        public Button backButton;
        public Button addMovieButton;
        public Button selectImageButton;
        public Image moviePosterImage;

        // Data
        private List<SelectableItem<string>> _allGenres = new();
        private List<SelectableItem<Director>> _allDirectors = new();
        private List<SelectableItem<Actor>> _allActors = new();
        private string _selectedImagePath;

        private bool _isInitialized = false;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += (s, e) => HandleBackButton();
                if (addMovieButton != null) addMovieButton.Click += (s, e) => HandleAddMovieButtonClick();
                if (selectImageButton != null) selectImageButton.Click += (s, e) => HandleSelectImageButton();

                if (searchGenreField != null) searchGenreField.TextChanged += (s, e) => FilterGenres();
                if (searchDirectorField != null) searchDirectorField.TextChanged += (s, e) => FilterDirectors();
                if (searchActorField != null) searchActorField.TextChanged += (s, e) => FilterActors();

                _isInitialized = true;
            }

            LoadData();
        }

        // ... (LoadData and Filter methods remain same) ...

        private void LoadData()
        {
            try
            {
                // Genres
                var genreNames = Enum.GetNames(typeof(MovieGenre));
                _allGenres = genreNames.Select(g => new SelectableItem<string>(g)).ToList();
                if (genreListView != null) genreListView.ItemsSource = _allGenres;

                // Directors
                try
                {
                    var directorsObj = _directorService.GetAllDirectors();
                    if (directorsObj is List<Director> directors)
                    {
                        _allDirectors = directors.Select(d => new SelectableItem<Director>(d)).ToList();
                        if (directorListView != null) directorListView.ItemsSource = _allDirectors;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load directors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Actors
                try
                {
                    var actorsObj = _actorService.GetAllActors();
                    if (actorsObj is List<Actor> actors)
                    {
                        _allActors = actors.Select(a => new SelectableItem<Actor>(a)).ToList();
                        if (actorListView != null) actorListView.ItemsSource = _allActors;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to load actors: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred while loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterGenres()
        {
            if (genreListView == null) return;
            var query = searchGenreField?.Text?.Trim().ToLower() ?? "";
            genreListView.ItemsSource = string.IsNullOrEmpty(query) 
                ? _allGenres 
                : _allGenres.Where(i => i.Item.ToLower().Contains(query)).ToList();
        }

        private void FilterDirectors()
        {
            if (directorListView == null) return;
            var query = searchDirectorField?.Text?.Trim().ToLower() ?? "";
            directorListView.ItemsSource = string.IsNullOrEmpty(query)
                ? _allDirectors
                : _allDirectors.Where(i => (i.Item.FirstName + " " + i.Item.LastName).ToLower().Contains(query)).ToList();
        }

        private void FilterActors()
        {
            if (actorListView == null) return;
            var query = searchActorField?.Text?.Trim().ToLower() ?? "";
            actorListView.ItemsSource = string.IsNullOrEmpty(query)
                ? _allActors
                : _allActors.Where(i => (i.Item.FirstName + " " + i.Item.LastName).ToLower().Contains(query)).ToList();
        }

        public void HandleSelectImageButton()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                _selectedImagePath = openFileDialog.FileName;
                if (moviePosterImage != null)
                {
                    moviePosterImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_selectedImagePath));
                }
            }
        }

        public void HandleAddMovieButtonClick()
        {
            if (string.IsNullOrWhiteSpace(movieNameField.Text) ||
                string.IsNullOrWhiteSpace(movieDurationField.Text) ||
                string.IsNullOrWhiteSpace(movieLanguageField.Text) ||
                string.IsNullOrWhiteSpace(movieRatedField.Text))
            {
                MessageBox.Show("Please enter complete information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(movieDurationField.Text.Trim(), out var duration))
            {
                MessageBox.Show("Duration must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(movieRatedField.Text.Trim(), out var rated))
            {
                MessageBox.Show("Rated must be a number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedGenres = _allGenres.Where(i => i.IsSelected).Select(i => Enum.Parse<MovieGenre>(i.Item)).ToList();
            var selectedDirectorItem = _allDirectors.FirstOrDefault(i => i.IsSelected);
            
            if (selectedDirectorItem == null || selectedDirectorItem.Item == null)
            {
                MessageBox.Show("Please select a director", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            var movie = new Movie
            {
                Id = Guid.NewGuid(),
                Name = movieNameField.Text.Trim(),
                Description = movieDescriptionField.Text.Trim(),
                Language = movieLanguageField.Text.Trim(),
                Duration = duration,
                Rated = rated,
                Premiere = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Genres = selectedGenres,
                DirectorId = selectedDirectorItem.Item.Id
            };

            try
            {
                // Save Image
                if (!string.IsNullOrEmpty(_selectedImagePath))
                {
                    try 
                    {
                        var destPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", $"{movie.Id}.jpg");
                        // Ensure directory exists
                        var dir = Path.GetDirectoryName(destPath);
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        // Load and convert image
                        var bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(_selectedImagePath);
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.EndInit();

                        var encoder = new JpegBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));
                        encoder.QualityLevel = 90;

                        using (var stream = new FileStream(destPath, FileMode.Create))
                        {
                            encoder.Save(stream);
                        }
                        MessageBox.Show($"[DEBUG] Image successfully saved to: {destPath}", "Debug Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save image: {ex.Message}", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }

                _movieService.InsertMovie(movie);

                // Insert movie actors using the movie ID we generated
                var selectedActors = _allActors.Where(i => i.IsSelected).Select(i => i.Item.Id).ToList();
                if (selectedActors.Any())
                {
                    _movieActorService.InsertMovieActors(movie.Id, selectedActors);
                }

                _movieListController.RefreshData();
                _screenController.NavigateTo<MovieList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the movie: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<MovieList>();
        }
    }
}
