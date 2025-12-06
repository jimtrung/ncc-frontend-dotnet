using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Theater_Management_FE.Views;
using Theater_Management_FE.Services;
using System.Windows.Media;

using Theater_Management_FE.Models;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class TinTucController
    {
        private ScreenController _screenController;

        // UI elements - Public for ControllerBinder matching HomePageUser
        public WrapPanel NewsWrapPanel;
        public Button BtnPrev;
        public Button BtnNext;
        
        // Navigation Buttons
        public Button homeButton;
        public Button showTimeButton;
        public Button newsButton;
        public Button promotionButton;
        public Button priceButton;
        public Button aboutButton;
        
        // Header UI
        public Button profileButton;
        public Button logoutButton;
        public Button bookTicketButton; 
        public TextBlock usernameText;

        private List<NewsItem> newsList = new();
        private int currentPage = 0;
        private readonly int itemsPerPage = 8;
        private bool _isInitialized = false;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        private AuthService authService;
        private AuthTokenUtil authTokenUtil;

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
                    BtnPrev.Click += HandlePrev;
                    BtnNext.Click += HandleNext;
                    
                    homeButton.Click += (s, e) => _screenController.NavigateTo<HomePageUser>();
                    showTimeButton.Click += (s, e) => _screenController.NavigateTo<ShowtimePage>();
                    promotionButton.Click += (s, e) => _screenController.NavigateTo<EventList>();
                    bookTicketButton.Click += (s, e) => _screenController.NavigateTo<BookedTicket>();
                    priceButton.Click += (s, e) => _screenController.NavigateTo<Price>();
                    
                    profileButton.Click += HandleProfileButton;
                    logoutButton.Click += HandleLogOutButton;

                    LoadData();
                    _isInitialized = true;
                }

                CheckUserStatus();
                ShowPage(currentPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tin tức: {ex.Message}");
            }
        }

        private void CheckUserStatus()
        {
            // Check xem đã đăng nhập chưa
            User user = null;
            try
            {
                var result = authService.GetUser();
                user = result as User;
            }
            catch { }
            UpdateUserUI(user);
        }

        public void HandleProfileButton(object sender, RoutedEventArgs e)
        {
             _screenController.NavigateTo<Profile>(); 
        }

        public void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            authTokenUtil.ClearRefreshToken();
            authTokenUtil.ClearAccessToken();
            UpdateUserUI(null);
            _screenController.NavigateTo<Home>();
        }

        private void UpdateUserUI(User user)
        {
            if (usernameText == null) return;

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

        // ========================= LOAD DATA =========================
        private void LoadData()
        {
            newsList.Add(new NewsItem("24/10/2025", "Gian hàng Trung tâm Chiếu phim Quốc gia chính thức góp mặt tại Hội chợ Mùa Thu 2025.", "card1.jpg"));
            newsList.Add(new NewsItem("13/10/2025", "CINETOUR 'Tay Anh Giữ Một Vì Sao' tại Trung tâm Chiếu phim Quốc gia ngày 10/10/2025.", "card2.jpg"));
            newsList.Add(new NewsItem("10/10/2025", "Cinetour 'Tay Anh Giữ Một Vì Sao' tại NCC ngày 9/10/2025.", "card3.jpg"));
            newsList.Add(new NewsItem("06/10/2025", "Đại hội Đại biểu Đảng bộ chính phủ lần thứ I, nhiệm kỳ 2025-2030", "card4.jpg"));
            newsList.Add(new NewsItem("02/10/2025", "TRUNG THU NÀY ĐẾN TRUNG TÂM CHIẾU PHIM QUỐC GIA NHẬN QUÀ CHO BÉ", "card5.jpg"));
            newsList.Add(new NewsItem("30/09/2025", "Úm ba la… dàn trai đẹp của Tử Chiến Trên Không đã chính thức “đổ bộ” tại Trung tâm Chiếu phim Quốc gia", "card6.jpg"));
            newsList.Add(new NewsItem("29/09/2025", "HOẠT ĐỘNG GIÁO DỤC – TRẢI NGHIỆM…", "card7.jpg"));
            newsList.Add(new NewsItem("24/09/2025", "Buổi ra mắt và họp báo bộ phim…", "card8.jpg"));

            // Trang 2
             newsList.Add(new NewsItem("24/10/2025", "Gian hàng NCC…", "card9.jpg"));
             newsList.Add(new NewsItem("13/10/2025", "CINETOUR…", "card10.jpg"));
             newsList.Add(new NewsItem("10/10/2025", "Cinetour NCC…", "card11.jpg"));
             newsList.Add(new NewsItem("06/10/2025", "Đại hội Đảng…", "card12.jpg"));
             newsList.Add(new NewsItem("02/10/2025", "TRUNG THU…", "card13.jpg"));
             newsList.Add(new NewsItem("30/09/2025", "Úm ba la…", "card14.jpg"));
             newsList.Add(new NewsItem("29/09/2025", "Trải nghiệm…", "card15.jpg"));
             newsList.Add(new NewsItem("24/09/2025", "Họp báo…", "card16.jpg"));
        }

        // ========================= PAGINATION =========================
        private void HandlePrev(object sender, RoutedEventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                ShowPage(currentPage);
            }
        }

        private void HandleNext(object sender, RoutedEventArgs e)
        {
            if ((currentPage + 1) * itemsPerPage < newsList.Count)
            {
                currentPage++;
                ShowPage(currentPage);
            }
        }

        private void ShowPage(int page)
        {
            NewsWrapPanel.Children.Clear();

            int start = page * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, newsList.Count);

            for (int i = start; i < end; i++)
            {
                var item = newsList[i];
                NewsWrapPanel.Children.Add(CreateNewsCard(item));
            }
        }

        // ========================= BUILD CARD UI =========================
        private Border CreateNewsCard(NewsItem item)
        {
            var card = new Border
            {
                Width = 300,
                CornerRadius = new CornerRadius(10),
                Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#1E2433"), // CardBackground
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            // Add Shadow Effect
            var shadow = new System.Windows.Media.Effects.DropShadowEffect
            {
                Color = Colors.Black,
                Opacity = 0.4,
                BlurRadius = 10,
                ShadowDepth = 3
            };
            card.Effect = shadow;

            var panel = new StackPanel();

            var img = new Image
            {
                Height = 180,
                Width = 300,
                Stretch = Stretch.UniformToFill
            };

            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                bitmap.UriSource = new Uri($"pack://application:,,,/Resources/Images/{item.ImageUrl}", UriKind.Absolute);
                bitmap.EndInit();
            }
            catch
            {
                 // Fallback logic
                 bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.UriSource = new Uri("pack://application:,,,/Resources/Images/default.jpg", UriKind.Absolute);
                 bitmap.EndInit();
            }

            img.Source = bitmap;

            var date = new TextBlock
            {
                Text = item.Date,
                Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#A0AEC0"), // SecondaryText
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 0)
            };

            var title = new TextBlock
            {
                Text = item.Title,
                TextWrapping = TextWrapping.Wrap,
                Foreground = System.Windows.Media.Brushes.White, // PrimaryText
                FontSize = 14,
                Margin = new Thickness(0, 5, 0, 0)
            };

            panel.Children.Add(img);
            panel.Children.Add(date);
            panel.Children.Add(title);

            card.Child = panel;
            return card;
        }

        // ========================= MODEL =========================
        private class NewsItem
        {
            public string Date { get; }
            public string Title { get; }
            public string ImageUrl { get; }

            public NewsItem(string date, string title, string imageUrl)
            {
                Date = date;
                Title = title;
                ImageUrl = imageUrl;
            }
        }
    }
}
