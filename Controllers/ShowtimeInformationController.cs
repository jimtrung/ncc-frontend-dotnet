using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Controllers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class ShowtimeInformationController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private ShowtimeService _showtimeService;
        private MovieService _movieService;
        private AuditoriumService _auditoriumService;
        private ShowtimeListController _showtimeListController;
        private Guid _showtimeId;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetShowtimeService(ShowtimeService service) => _showtimeService = service;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetShowtimeListController(ShowtimeListController controller) => _showtimeListController = controller;
        public void SetShowtimeId(Guid id) => _showtimeId = id;

        public TextBox showtimeMovieNameField;
        public TextBox showtimeAuditoriumNameField;
        public TextBox showtimeStartTimeField;
        public TextBox showtimeEndTimeField;
        public TextBox showtimeDateField;
        public TextBox showtimeQuantityField;
        public Button backButton;
        public Button deleteButton;

        public bool _isInitialized = false;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += (s, e) => HandleBackButton();
                // if (editButton != null) editButton.Click += (s, e) => HandleEditButton();
                if (deleteButton != null) deleteButton.Click += (s, e) => HandleDeleteButton();
                _isInitialized = true;
            }
            try
            {
                var showtime = _showtimeService.GetShowtimeById(_showtimeId);
                var movie = _movieService.GetMovieById(showtime.MovieId);
                var auditorium = _auditoriumService.GetAuditoriumById(showtime.AuditoriumId);
                var _quantity = auditorium != null ? auditorium.Capacity : 0;

                showtimeMovieNameField.Text = movie.Name;
                showtimeAuditoriumNameField.Text = auditorium.Name;
                showtimeStartTimeField.Text = showtime.StartTimeString.ToString();
                showtimeEndTimeField.Text = showtime.EndTimeString.ToString();
                showtimeDateField.Text = showtime.ShowDateString.ToString();
                showtimeQuantityField.Text = _quantity.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load showtime information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<ShowtimeList>();
        }

        public void HandleDeleteButton()
        {
            var result = MessageBox.Show("Are you sure you want to delete this showtime?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _showtimeService.DeleteShowtimeById(_showtimeId);
                    MessageBox.Show("Showtime deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _showtimeListController.HandleOnOpen();
                    _screenController.NavigateTo<ShowtimeList>();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to delete showtime: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}