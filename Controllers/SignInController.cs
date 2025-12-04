using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
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
        public TextBox visiblePasswordField;
        public CheckBox showPasswordCheckBox;
        public Button signInButton;
        public Button backButton;
        public Hyperlink forgotPasswordLink;

        public void HandleOnOpen()
        {
            User user = null;
            try { user = (User)_authService.GetUser(); } catch { }

            if (user != null)
            {
                if (user.Role == UserRole.user) _screenController.NavigateTo<HomePageUser>();
                if (user.Role == UserRole.administrator) _screenController.NavigateTo<HomePageManager>();
                return;
            }

            // Reset password fields and checkbox state when page opens
            if (passwordField != null)
            {
                passwordField.Clear();
                passwordField.Visibility = Visibility.Visible;
            }
            if (visiblePasswordField != null)
            {
                visiblePasswordField.Clear();
                visiblePasswordField.Visibility = Visibility.Collapsed;
            }
            if (showPasswordCheckBox != null)
            {
                showPasswordCheckBox.IsChecked = false;
            }
            if (usernameField != null)
            {
                usernameField.Clear();
                usernameField.Focus();
            }
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleSignInButton(object sender, RoutedEventArgs e)
        {
            PerformSignIn();
        }

        private void PerformSignIn()
        {
            if (showPasswordCheckBox.IsChecked == true)
            {
                passwordField.Password = visiblePasswordField.Text;
            }

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

                // Clear fields
                usernameField.Clear();
                passwordField.Clear();
                visiblePasswordField.Clear();
                showPasswordCheckBox.IsChecked = false;

                // Navigate based on role
                if (tokenPair.AccessToken != null)
                {
                    // Decode token to get role or fetch user
                    // Since we just have the token, let's fetch the user to be sure about the role
                    // Or we can decode the token if we had a helper for that.
                    // But fetching user is safer and we have the token saved.
                    
                    try 
                    {
                        var currentUser = (User)_authService.GetUser();
                        if (currentUser.Role == UserRole.administrator)
                        {
                            _screenController.NavigateTo<HomePageManager>();
                        }
                        else
                        {
                            _screenController.NavigateTo<HomePageUser>();
                        }
                        return;
                    }
                    catch 
                    {
                        // Fallback
                         _screenController.NavigateTo<Home>();
                    }
                }
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

        private void TogglePasswordVisibility(object sender, RoutedEventArgs e)
        {
            if (showPasswordCheckBox.IsChecked == true)
            {
                // Show password
                visiblePasswordField.Text = passwordField.Password;
                visiblePasswordField.Visibility = Visibility.Visible;
                passwordField.Visibility = Visibility.Collapsed;
                visiblePasswordField.Focus();
            }
            else
            {
                // Hide password
                passwordField.Password = visiblePasswordField.Text;
                visiblePasswordField.Visibility = Visibility.Collapsed;
                passwordField.Visibility = Visibility.Visible;
                passwordField.Focus();
            }
        }

        private void HandleKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                PerformSignIn();
            }
        }

        public void BindUIControls(TextBox usernameField, PasswordBox passwordField, TextBox visiblePasswordField, CheckBox showPasswordCheckBox, Button signInButton, Button backButton, Hyperlink forgotPasswordLink)
        {
            this.usernameField = usernameField;
            this.passwordField = passwordField;
            this.visiblePasswordField = visiblePasswordField;
            this.showPasswordCheckBox = showPasswordCheckBox;
            this.signInButton = signInButton;
            this.backButton = backButton;
            this.forgotPasswordLink = forgotPasswordLink;

            this.signInButton.Click += HandleSignInButton;
            this.backButton.Click += HandleBackButton;
            this.forgotPasswordLink.Click += HandleForgotPassword;
            this.showPasswordCheckBox.Click += TogglePasswordVisibility;
            
            // Add Enter key support
            this.usernameField.KeyDown += HandleKeyDown;
            this.passwordField.KeyDown += HandleKeyDown;
            this.visiblePasswordField.KeyDown += HandleKeyDown;
        }
    }
}
