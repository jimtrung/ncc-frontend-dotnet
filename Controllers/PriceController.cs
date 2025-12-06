using System;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class PriceController
    {
        private ScreenController screenController;
        private AuthService authService;
        private AuthTokenUtil authTokenUtil;

        // Navigation buttons
        public Button homeButton;
        public Button showTimeButton;
        public Button bookTicketButton;
        public Button newsButton;
        public Button promotionButton;

        // User profile buttons
        public Button profileButton;
        public Button logoutButton;
        public TextBlock usernameText;

        private bool _isInitialized = false;

        public void SetScreenController(ScreenController controller)
        {
            screenController = controller;
        }

        public void SetAuthService(AuthService service)
        {
            authService = service;
        }

        public void SetAuthTokenUtil(AuthTokenUtil util)
        {
            authTokenUtil = util;
        }

        public void HandleOnOpen()
        {
            try
            {
                if (!_isInitialized)
                {
                    if (homeButton != null) homeButton.Click += (s, e) => screenController.NavigateTo<HomePageUser>();
                    if (showTimeButton != null) showTimeButton.Click += (s, e) => screenController.NavigateTo<ShowtimePage>();
                    if (bookTicketButton != null) bookTicketButton.Click += (s, e) => screenController.NavigateTo<BookedTicket>();
                    if (newsButton != null) newsButton.Click += (s, e) => screenController.NavigateTo<TinTuc>();
                    if (promotionButton != null) promotionButton.Click += (s, e) => screenController.NavigateTo<EventList>();

                    if (profileButton != null) profileButton.Click += HandleProfileButton;
                    if (logoutButton != null) logoutButton.Click += HandleLogOutButton;

                    _isInitialized = true;
                }

                // Check if required services are available
                if (authService == null || screenController == null)
                {
                    MessageBox.Show("Các dịch vụ yêu cầu chưa được khởi tạo", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Try to fetch user information
                User user = null;
                try
                {
                    var result = authService.GetUser();
                    user = result as User;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể lấy thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    screenController.NavigateTo<Home>();
                    return;
                }

                if (user == null)
                {
                    screenController.NavigateTo<Home>();
                    return;
                }

                UpdateUserUI(user);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong PriceController: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleProfileButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<Profile>();
        }

        public void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            authTokenUtil.ClearRefreshToken();
            authTokenUtil.ClearAccessToken();
            UpdateUserUI(null);
            screenController.NavigateTo<Home>();
        }

        private void UpdateUserUI(User user)
        {
            if (usernameText == null)
            {
                return;
            }

            if (user == null)
            {
                usernameText.Text = "Khách";
                if (logoutButton != null) logoutButton.Visibility = Visibility.Collapsed;
                if (profileButton != null) profileButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                usernameText.Text = user.Username ?? "Người dùng";
                if (logoutButton != null) logoutButton.Visibility = Visibility.Visible;
                if (profileButton != null) profileButton.Visibility = Visibility.Visible;
            }
        }
    }
}
