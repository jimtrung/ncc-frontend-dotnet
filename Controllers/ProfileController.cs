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

        public TextBox usernameField;
        public TextBox emailField;
        public TextBlock verifiedField;
        public System.Windows.Controls.Border verifiedBadge;
        public TextBox createdAtField;
        public TextBlock roleText;

        public Button backButton;
        public Button logOutButton;

        private bool _isInitialized = false;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthService(AuthService service) => _authService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                backButton.Click += HandleBackButton;
                logOutButton.Click += HandleLogOutButton;
                _isInitialized = true;
            }

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
                MessageBox.Show($"Không thể tải thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                _screenController.NavigateTo<Home>();
                return;
            }

            if (userInfo != null)
            {
                if (usernameField != null) usernameField.Text = userInfo.Username;
                if (emailField != null) emailField.Text = userInfo.Email;
                if (createdAtField != null) createdAtField.Text = userInfo.CreatedAt.ToString("dd/MM/yyyy HH:mm");

                if (roleText != null)
                {
                    roleText.Text = userInfo.Role == UserRole.administrator ? "Quản Trị Viên" : "Người Dùng";
                }

                if (verifiedField != null && verifiedBadge != null)
                {
                    if (userInfo.Verified)
                    {
                        verifiedField.Text = "Đã xác thực";
                        verifiedBadge.Background = new System.Windows.Media.SolidColorBrush(
                            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#4CAF50"));
                    }
                    else
                    {
                        verifiedField.Text = "Chưa xác thực";
                        verifiedBadge.Background = new System.Windows.Media.SolidColorBrush(
                            (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF9800"));
                    }
                }
            }
            else
            {
                MessageBox.Show("Không thể tải thông tin người dùng", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                _screenController.NavigateTo<Home>();
            }
        }

        public void HandleBackButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Home>();
        }

        public void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            _authTokenUtil.ClearAccessToken();
            _authTokenUtil.ClearRefreshToken();
            _screenController.NavigateTo<Home>();
        }
    }
}
