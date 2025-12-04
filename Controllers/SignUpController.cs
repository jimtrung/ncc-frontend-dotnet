using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class SignUpController
    {
        private ScreenController _screenController;
        private AuthService _authService;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthService(AuthService authService)
        {
            _authService = authService;
        }

        public TextBox usernameField;
        public TextBox emailField;
        public PasswordBox passwordField;
        public TextBox visiblePasswordField;
        public CheckBox showPasswordCheckBox;
        public Button signUpButton;
        public Button backButton;

        public void HandleOnOpen()
        {
            // Check if user is already logged in
            User user = null;
            try { user = (User)_authService.GetUser(); } catch { }

            if (user != null)
            {
                if (user.Role == UserRole.user) _screenController.NavigateTo<HomePageUser>();
                if (user.Role == UserRole.administrator) _screenController.NavigateTo<HomePageManager>();
                return;
            }

            // Reset all fields when page opens
            if (usernameField != null) 
            {
                usernameField.Clear();
                usernameField.Focus();
            }
            if (emailField != null) emailField.Clear();
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
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleSignUpButton(object sender, RoutedEventArgs e)
        {
            PerformSignUp();
        }

        private void PerformSignUp()
        {
            // Ensure password is up to date from visible field if checked
            if (showPasswordCheckBox.IsChecked == true)
            {
                passwordField.Password = visiblePasswordField.Text;
            }

            var user = new User
            {
                Username = usernameField.Text,
                Email = emailField.Text,
                Password = passwordField.Password
            };

            object response;
            try
            {
                response = _authService.SignUp(user);
                if (response is ErrorResponse errRes)
                {
                    MessageBox.Show(errRes.Message, "Sign up error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to sign up: {ex.Message}", "Sign up error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Clear fields
            usernameField.Clear();
            emailField.Clear();
            passwordField.Clear();
            visiblePasswordField.Clear();
            showPasswordCheckBox.IsChecked = false;

            _screenController.NavigateTo<SignIn>();
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
                PerformSignUp();
            }
        }

        public void BindUIControls(TextBox usernameField, TextBox emailField, PasswordBox passwordField, TextBox visiblePasswordField, CheckBox showPasswordCheckBox, Button signUpButton, Button backButton)
        {
            this.usernameField = usernameField;
            this.emailField = emailField;
            this.passwordField = passwordField;
            this.visiblePasswordField = visiblePasswordField;
            this.showPasswordCheckBox = showPasswordCheckBox;
            this.signUpButton = signUpButton;
            this.backButton = backButton;

            this.signUpButton.Click += HandleSignUpButton;
            this.backButton.Click += HandleBackButton;
            this.showPasswordCheckBox.Click += TogglePasswordVisibility;
            
            // Add Enter key support
            this.usernameField.KeyDown += HandleKeyDown;
            this.emailField.KeyDown += HandleKeyDown;
            this.passwordField.KeyDown += HandleKeyDown;
            this.visiblePasswordField.KeyDown += HandleKeyDown;
        }
    }
}
