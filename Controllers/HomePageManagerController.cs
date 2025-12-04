using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Models;

namespace Theater_Management_FE.Controllers
{
    public class HomePageManagerController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;

        public Button movieButton;
        public Button profileButton;
        public Button auditoriumButton;
        public Button logOutButton;
        public Button showtimeButton;

        private bool _isInitialized = false;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthTokenUtil(AuthTokenUtil authTokenUtil)
        {
            _authTokenUtil = authTokenUtil;
        }

        private AuthService _authService;

        public void SetAuthService(AuthService authService)
        {
            _authService = authService;
        }

        public void HandleOnOpen()
        {
            // Security Check
            var token = _authTokenUtil.LoadAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                _screenController.NavigateTo<Home>();
                return;
            }

            try
            {
                var user = (User)_authService.GetUser();
                if (user.Role != UserRole.administrator)
                {
                    _screenController.NavigateTo<HomePageUser>();
                    return;
                }
            }
            catch
            {
                _screenController.NavigateTo<Home>();
                return;
            }

            if (!_isInitialized)
            {
                if (movieButton != null) movieButton.Click += HandleMovieButton;
                if (auditoriumButton != null) auditoriumButton.Click += HandleAuditoriumButton;
                if (profileButton != null) profileButton.Click += HandleProfileButton;
                if (logOutButton != null) logOutButton.Click += HandleLogOutButton;
                if (showtimeButton != null) showtimeButton.Click += HandleShowtimeButton;

                _isInitialized = true;
            }
        }

        private void HandleMovieButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<MovieList>();
        }

        private void HandleAuditoriumButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        private void HandleProfileButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Profile>();
        }

        private void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            _authTokenUtil.ClearRefreshToken();
            _authTokenUtil.ClearAccessToken();
            _screenController.NavigateTo<Home>();
        }

        private void HandleShowtimeButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<ShowtimeList>();
        }
    }
}
