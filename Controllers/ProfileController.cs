using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class ProfileController
    {
        private ScreenController _screenController;
        private AuthService _authService;
        private AuthTokenUtil _authTokenUtil;

        // Fields must match XAML x:Name attributes (camelCase)
        public TextBox usernameField;
        public TextBox emailField;
        public TextBox phoneNumberField;
        public PasswordBox passwordField;
        public TextBox verifiedField;
        public TextBox createdAtField;

        public Button backButton;
        public Button editButton;
        public Button logOutButton;

        private bool _isInitialized = false;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthService(AuthService service) => _authService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += HandleBackButton;
                if (editButton != null) editButton.Click += HandleEditButton;
                if (logOutButton != null) logOutButton.Click += HandleLogOutButton;
                _isInitialized = true;
            }

            // If there's no token, user is not logged in → go back to Home silently
            var token = _authTokenUtil.LoadAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                _screenController.NavigateTo<Home>();
                return;
            }

            User userInfo = null;
            try
            {
                userInfo = (User)_authService.GetUser();
            }
            catch (Exception ex) {
                MessageBox.Show($"Failed to fetch user info: {ex.Message}", "Fetch error", MessageBoxButton.OK, MessageBoxImage.Error);
                _screenController.NavigateTo<Home>();
                return;
            }

            if (userInfo != null)
            {
                if (usernameField != null) usernameField.Text = userInfo.Username;
                if (emailField != null) emailField.Text = userInfo.Email;
                if (phoneNumberField != null) phoneNumberField.Text = userInfo.PhoneNumber;
                if (passwordField != null) passwordField.Password = userInfo.Password;
                if (verifiedField != null) verifiedField.Text = userInfo.Verified.ToString();
                if (createdAtField != null) createdAtField.Text = userInfo.CreatedAt.ToString();
            }
            else
            {
                MessageBox.Show("Failed to fetch user info", "Fetch error", MessageBoxButton.OK, MessageBoxImage.Error);
                _screenController.NavigateTo<Home>();
            }
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleEditButton(object sender, RoutedEventArgs e)
        {
            // TODO: implement edit profile functionality
        }

        public void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            _authTokenUtil.ClearAccessToken();
            _authTokenUtil.ClearRefreshToken();
            _screenController.NavigateTo<Home>();
        }
    }
}
