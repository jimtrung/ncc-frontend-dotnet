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

        public PasswordBox confirmPasswordField;
        public TextBox visibleConfirmPasswordField;

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
            if (showPasswordCheckBox.IsChecked == true)
            {
                passwordField.Password = visiblePasswordField.Text;
                confirmPasswordField.Password = visibleConfirmPasswordField.Text;
            }

            // Validate password
            if (passwordField.Password != confirmPasswordField.Password)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!",
                                "Lỗi đăng ký",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(usernameField.Text) ||
                string.IsNullOrWhiteSpace(emailField.Text) ||
                string.IsNullOrWhiteSpace(passwordField.Password))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!",
                                "Thiếu thông tin",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(emailField.Text, emailPattern))
            {
                MessageBox.Show("Email không hợp lệ! Vui lòng sử dụng email dạng: example@gmail.com",
                                "Sai định dạng email",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            // Validate strong password
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            if (!System.Text.RegularExpressions.Regex.IsMatch(passwordField.Password, passwordPattern))
            {
                MessageBox.Show(
                    "Mật khẩu phải có tối thiểu 8 kí tự, bao gồm:\n" +
                    "- Chữ thường\n" +
                    "- Chữ hoa\n" +
                    "- Chữ số\n" +
                    "- Ký tự đặc biệt (@$!%*?&)",
                    "Mật khẩu yếu",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
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

                // Show confirm password
                visibleConfirmPasswordField.Text = confirmPasswordField.Password;
                visibleConfirmPasswordField.Visibility = Visibility.Visible;
                confirmPasswordField.Visibility = Visibility.Collapsed;
                visiblePasswordField.Focus();
            }
            else
            {
                // Hide password
                passwordField.Password = visiblePasswordField.Text;
                visiblePasswordField.Visibility = Visibility.Collapsed;
                passwordField.Visibility = Visibility.Visible;

                confirmPasswordField.Password = visibleConfirmPasswordField.Text;
                visibleConfirmPasswordField.Visibility = Visibility.Collapsed;
                confirmPasswordField.Visibility = Visibility.Visible;
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

        public void BindUIControls(TextBox usernameField, TextBox emailField, PasswordBox passwordField, TextBox visiblePasswordField, PasswordBox confirmPasswordField,
    TextBox visibleConfirmPasswordField, CheckBox showPasswordCheckBox, Button signUpButton, Button backButton)
        {
            this.usernameField = usernameField;
            this.emailField = emailField;
            this.passwordField = passwordField;
            this.visiblePasswordField = visiblePasswordField;

            this.confirmPasswordField = confirmPasswordField;
            this.visibleConfirmPasswordField = visibleConfirmPasswordField;

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
            this.confirmPasswordField.KeyDown += HandleKeyDown;
            this.visibleConfirmPasswordField.KeyDown += HandleKeyDown;
        }
    }
}
