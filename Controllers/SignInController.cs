using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class SignInController
    {
        private ScreenController _screenController;
        private AuthService _authService;
        private AuthTokenUtil _authTokenUtil;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthService(AuthService authService)
        {
            _authService = authService;
        }

        public void SetAuthTokenUtil(AuthTokenUtil authTokenUtil)
        {
            _authTokenUtil = authTokenUtil;
        }

        public TextBox usernameField;
        public PasswordBox passwordField;
        public Button signInButton;
        public Button backButton;
        public Hyperlink forgotPasswordLink;

        public void HandleOnOpen()
        {
            User user = null;
            try { user = (User)_authService.GetUser(); } catch { }

            if (user != null)
            {
                if (user.Role == UserRole.USER) _screenController.NavigateTo<HomePageUser>();
                if (user.Role == UserRole.ADMINISTRATOR) _screenController.NavigateTo<HomePageManager>();
            }
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleSignInButton(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Username = usernameField.Text,
                Password = passwordField.Password
            };

            object response;
            try
            {
                response = _authService.SignIn(user);
                if (response is ErrorResponse errRes)
                {
                    MessageBox.Show(errRes.Message, "Sign in error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var tokenPair = (TokenPair)response;
                _authTokenUtil.SaveAccessToken(tokenPair.AccessToken);
                _authTokenUtil.SaveRefreshToken(tokenPair.RefreshToken);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to sign in: {ex.Message}", "Sign in error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _screenController.NavigateTo<Home>();
        }

        public void HandleForgotPassword(object sender, RoutedEventArgs e)
        {
        }

        public void BindUIControls(TextBox usernameField, PasswordBox passwordField, Button signInButton, Button backButton, Hyperlink forgotPasswordLink)
        {
            this.usernameField = usernameField;
            this.passwordField = passwordField;
            this.signInButton = signInButton;
            this.backButton = backButton;
            this.forgotPasswordLink = forgotPasswordLink;

            this.signInButton.Click += HandleSignInButton;
            this.backButton.Click += HandleBackButton;
            this.forgotPasswordLink.Click += HandleForgotPassword;
        }
    }
}
