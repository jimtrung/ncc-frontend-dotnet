using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

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
        public TextBox phoneNumberField;
        public PasswordBox passwordField;
        public Button signUpButton;
        public Button backButton;

        public void HandleOnOpen()
        {
            User user = null;
            try { user = (User)_authService.GetUser(); } catch { }

            if (user != null)
            {
                if (user.Role == UserRole.user) _screenController.NavigateTo<HomePageUser>();
                if (user.Role == UserRole.administrator) _screenController.NavigateTo<HomePageManager>();
            }
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleSignUpButton(object sender, RoutedEventArgs e)
        {
            var user = new User
            {
                Username = usernameField.Text,
                Email = emailField.Text,
                PhoneNumber = phoneNumberField.Text,
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
            catch
            {
                MessageBox.Show("Failed to sign up", "Sign up error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _screenController.NavigateTo<SignIn>();
        }

        public void BindUIControls(TextBox usernameField, TextBox emailField, TextBox phoneNumberField, PasswordBox passwordField, Button signUpButton, Button backButton)
        {
            this.usernameField = usernameField;
            this.emailField = emailField;
            this.phoneNumberField = phoneNumberField;
            this.passwordField = passwordField;
            this.signUpButton = signUpButton;
            this.backButton = backButton;

            this.signUpButton.Click += HandleSignUpButton;
            this.backButton.Click += HandleBackButton;
        }
    }
}
