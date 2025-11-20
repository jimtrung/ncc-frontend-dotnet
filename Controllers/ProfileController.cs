using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class ProfileController
    {
        private ScreenController _screenController;
        private AuthService _authService;
        private AuthTokenUtil _authTokenUtil;

        public TextBox UsernameField;
        public TextBox EmailField;
        public TextBox PhoneNumberField;
        public PasswordBox PasswordField;
        public TextBox VerifiedField;
        public TextBox CreatedAtField;

        public Button BackButton;
        public Button EditButton;
        public Button LogOutButton;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthService(AuthService service) => _authService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;

        public void HandleOnOpen()
        {
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
                UsernameField.Text = userInfo.Username;
                EmailField.Text = userInfo.Email;
                PhoneNumberField.Text = userInfo.PhoneNumber;
                PasswordField.Password = userInfo.Password;
                VerifiedField.Text = userInfo.Verified.ToString();
                CreatedAtField.Text = userInfo.CreatedAt.ToString();
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

        public void BindUIControls(TextBox username, TextBox email, TextBox phone, PasswordBox password, TextBox verified, TextBox createdAt, Button back, Button edit, Button logout)
        {
            UsernameField = username;
            EmailField = email;
            PhoneNumberField = phone;
            PasswordField = password;
            VerifiedField = verified;
            CreatedAtField = createdAt;
            BackButton = back;
            EditButton = edit;
            LogOutButton = logout;

            BackButton.Click += HandleBackButton;
            EditButton.Click += HandleEditButton;
            LogOutButton.Click += HandleLogOutButton;
        }
    }
}
