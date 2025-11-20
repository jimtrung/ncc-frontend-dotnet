using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class HomePageManagerController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;

        public Button MovieButton;
        public Button profileButton;
        public Button auditoriumButton;
        public Button showtimeButton;
        public Button logoutButton;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthTokenUtil(AuthTokenUtil authTokenUtil)
        {
            _authTokenUtil = authTokenUtil;
        }

        public void BindUIControls(Button movieBtn, Button auditoriumBtn, Button profileBtn, Button logoutBtn)
        {
            MovieButton = movieBtn;
            auditoriumButton = auditoriumBtn;
            profileButton = profileBtn;
            logoutButton = logoutBtn;

            MovieButton.Click += HandleMovieButton;
            auditoriumButton.Click += HandleAuditoriumButton;
            profileButton.Click += HandleProfileButton;
            logoutButton.Click += HandleLogOutButton;
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
    }
}
