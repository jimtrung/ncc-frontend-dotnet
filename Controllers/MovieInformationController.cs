using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Controllers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class MovieInformationController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private DirectorService _directorService;
        private MovieListController _movieListController;
        private Guid _movieId;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetDirectorService(DirectorService service) => _directorService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetMovieListController(MovieListController controller) => _movieListController = controller;
        public void SetMovieId(Guid id) => _movieId = id;

        public TextBox movieNameField;
        public TextBox movieDescriptionField;
        public TextBox movieDirectorIdField;
        public TextBox movieGenresField;
        public TextBox moviePremiereField;
        public TextBox movieDurationField;
        public TextBox movieLanguageField;
        public TextBox movieRatedField;

        public Button backButton;
        public Button editButton;
        public Button deleteButton;

        private bool _isInitialized = false;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += (s, e) => HandleBackButton();
                if (editButton != null) editButton.Click += (s, e) => HandleEditButton();
                if (deleteButton != null) deleteButton.Click += (s, e) => HandleDeleteButton();
                _isInitialized = true;
            }

            try
            {
                var movie = _movieService.GetMovieById(_movieId);
                var id = movie.DirectorId;

                Director director = null;
                if (id.HasValue) // kiểm tra nullable
                {
                    director = _directorService.GetDirectorById(id.Value);
                }

                // bây giờ director có thể null nếu movie không có director
                if (movieDirectorIdField != null)
                {
                    movieDirectorIdField.Text = director.FullName;
                }

                if (movie == null)
                {
                    MessageBox.Show("Không tìm thấy phim.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (movieNameField != null) movieNameField.Text = movie.Name;
                if (movieDescriptionField != null) movieDescriptionField.Text = movie.Description;
                if (movieLanguageField != null) movieLanguageField.Text = movie.Language;
                if (movieDurationField != null) movieDurationField.Text = movie.Duration.ToString();
                if (movieRatedField != null) movieRatedField.Text = movie.Rated.ToString();
                if (moviePremiereField != null) moviePremiereField.Text = movie.Premiere?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                if (movieGenresField != null) movieGenresField.Text = string.Join(", ", movie.Genres);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể tải chi tiết phim: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<MovieList>();
        }

        public void HandleEditButton()
        {
            try
            {
                var movie_ = _movieService.GetMovieById(_movieId);
                var id = movie_.DirectorId;
                var updatedMovie = new Movie
                {
                    Id = _movieId,
                    Name = movieNameField.Text.Trim(),
                    Description = movieDescriptionField.Text.Trim(),
                    Language = movieLanguageField.Text.Trim(),
                    UpdatedAt = DateTime.UtcNow,
                    DirectorId = id
                };

                // Kiểm tra Rated
                if (!int.TryParse(movieRatedField.Text.Trim(), out var rated))
                {
                    MessageBox.Show("Độ tuổi giới hạn phải là số nguyên.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // stop update

                }
                updatedMovie.Rated = rated;

                // Kiểm tra Duration
                if (!int.TryParse(movieDurationField.Text.Trim(), out var duration))
                {
                    MessageBox.Show("Thời lượng phải là số nguyên.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; // stop update
                }
                updatedMovie.Duration = duration;

                // Kiểm tra Premiere
                if (!DateTime.TryParse(moviePremiereField.Text.Trim(), out var premiere))
                {
                    MessageBox.Show("Premiere phải đúng định dạng yyyy-MM-dd HH:mm:ss.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                updatedMovie.Premiere = DateTime.SpecifyKind(premiere, DateTimeKind.Utc);

                // Xử lý genres
                var genresText = movieGenresField.Text.Trim();
                if (!string.IsNullOrEmpty(genresText))
                {
                    var genreStrings = genresText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(s => s.Trim());

                    var genres = new List<MovieGenre>();
                    foreach (var g in genreStrings)
                    {
                        if (Enum.TryParse<MovieGenre>(g, true, out var genre))
                            genres.Add(genre);
                        else
                        {
                            MessageBox.Show($"Thể loại '{g}' không hợp lệ.", "Lỗi nhập liệu", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;

                        }
                    }
                    updatedMovie.Genres = genres;
                }

                _movieService.UpdateMovie(_movieId, updatedMovie);
                MessageBox.Show("Cập nhật phim thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                var movie = _movieService.GetMovieById(_movieId);
                _movieListController.UpdateMovie(movie);
                _screenController.NavigateTo<MovieList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể cập nhật phim: {ex.Message}\n\nStack: {ex.StackTrace}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public void HandleDeleteButton()
        {
            try
            {
                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa phim này không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _movieService.DeleteMovieById(_movieId);
                    _movieListController.RefreshData();
                    _screenController.NavigateTo<MovieList>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xóa phim: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
