using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Theater_Management_FE.Views;
using Theater_Management_FE.Services;
using System.Windows.Media;

namespace Theater_Management_FE.Controllers
{
    public class TinTucController
    {
        private ScreenController _screenController;

        // UI elements
        private WrapPanel newsWrapPanel;
        private Button btnPrev;
        private Button btnNext;

        private Button btnBackHome;

        private List<NewsItem> newsList = new();
        private int currentPage = 0;
        private readonly int itemsPerPage = 8;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }
        public void HandleBackToHome(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<HomePageUser>();
        }

        // ========================= BIND UI =========================
        // Parameter names MUST match XAML x:Name attributes exactly (case-sensitive) for ControllerBinder to work
        public void BindUIControls(WrapPanel NewsWrapPanel, Button BtnPrev, Button BtnNext, Button btnBackHome)
        {
            this.newsWrapPanel = NewsWrapPanel;
            this.btnPrev = BtnPrev;
            this.btnNext = BtnNext;
            this.btnBackHome = btnBackHome;
            btnBackHome.Click += HandleBackToHome;
            BtnPrev.Click += HandlePrev;
            BtnNext.Click += HandleNext;

            LoadData();
            ShowPage(currentPage);
        }

        // ========================= LOAD DATA =========================
        private void LoadData()
        {
            newsList.Add(new NewsItem("24/10/2025", "Gian hàng Trung tâm Chiếu phim Quốc gia chính thức góp mặt tại Hội chợ Mùa Thu 2025.", "Images/card1.jpg"));
            newsList.Add(new NewsItem("13/10/2025", "CINETOUR 'Tay Anh Giữ Một Vì Sao' tại Trung tâm Chiếu phim Quốc gia ngày 10/10/2025.", "Images/card2.jpg"));
            newsList.Add(new NewsItem("10/10/2025", "Cinetour 'Tay Anh Giữ Một Vì Sao' tại NCC ngày 9/10/2025.", "Images/card3.jpg"));
            newsList.Add(new NewsItem("06/10/2025", "Đại hội Đại biểu Đảng bộ chính phủ lần thứ I, nhiệm kỳ 2025-2030", "Images/card4.jpg"));
            newsList.Add(new NewsItem("02/10/2025", "TRUNG THU NÀY ĐẾN TRUNG TÂM CHIẾU PHIM QUỐC GIA NHẬN QUÀ CHO BÉ", "Images/card5.jpg"));
            newsList.Add(new NewsItem("30/09/2025", "Úm ba la… dàn trai đẹp của Tử Chiến Trên Không đã chính thức “đổ bộ” tại Trung tâm Chiếu phim Quốc gia", "Images/card6.jpg"));
            newsList.Add(new NewsItem("29/09/2025", "HOẠT ĐỘNG GIÁO DỤC – TRẢI NGHIỆM…", "Images/card7.jpg"));
            newsList.Add(new NewsItem("24/09/2025", "Buổi ra mắt và họp báo bộ phim…", "Images/card8.jpg"));

            // Trang 2
            // newsList.Add(new NewsItem("24/10/2025", "Gian hàng NCC…", "Images/card9.jpg"));
            // newsList.Add(new NewsItem("13/10/2025", "CINETOUR…", "Images/card10.jpg"));
            // newsList.Add(new NewsItem("10/10/2025", "Cinetour NCC…", "Images/card11.jpg"));
            // newsList.Add(new NewsItem("06/10/2025", "Đại hội Đảng…", "Images/card12.jpg"));
            // newsList.Add(new NewsItem("02/10/2025", "TRUNG THU…", "Images/card13.jpg"));
            // newsList.Add(new NewsItem("30/09/2025", "Úm ba la…", "Images/card14.jpg"));
            // newsList.Add(new NewsItem("29/09/2025", "Trải nghiệm…", "Images/card15.jpg"));
            // newsList.Add(new NewsItem("24/09/2025", "Họp báo…", "Images/card16.jpg"));
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
            newsWrapPanel.Children.Clear();

            int start = page * itemsPerPage;
            int end = Math.Min(start + itemsPerPage, newsList.Count);

            for (int i = start; i < end; i++)
            {
                var item = newsList[i];
                newsWrapPanel.Children.Add(CreateNewsCard(item));
            }
        }

        // ========================= BUILD CARD UI =========================
        private Border CreateNewsCard(NewsItem item)
        {
            var card = new Border
            {
                Width = 300,
                CornerRadius = new CornerRadius(10),
                Background = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#1c1e24"),
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            var panel = new StackPanel();

            var img = new Image
            {
                Height = 180,
                Width = 300,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
                Source = new BitmapImage(new Uri(item.ImageUrl, UriKind.Relative))
            };

            var date = new TextBlock
            {
                Text = item.Date,
                Foreground = (System.Windows.Media.Brush)new BrushConverter().ConvertFrom("#aaaaaa"),
                FontSize = 12,
                Margin = new Thickness(0, 5, 0, 0)
            };

            var title = new TextBlock
            {
                Text = item.Title,
                TextWrapping = TextWrapping.Wrap,
                Foreground = System.Windows.Media.Brushes.White,
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
