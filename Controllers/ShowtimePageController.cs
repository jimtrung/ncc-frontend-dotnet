using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class ShowtimePageController
    {
        private ScreenController screenController;
        private AuthService authService;
        private MovieService movieService;
        private AuditoriumService auditoriumService;
        public ShowtimeService showtimeService;
        private AuthTokenUtil authTokenUtil;
        private Guid _uuid;

        private static readonly Random _rand = new Random();

        public WrapPanel showtimeList;
        public Button logoutButton;
        public Button homeButton;
        public Button bookedTicketButton;
        public Button newsButton;
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

        public void SetMovieService(MovieService service)
        {
            movieService = service;
        }

        public void SetAuthTokenUtil(AuthTokenUtil util)
        {
            authTokenUtil = util;
        }
        public void SetShowtimeService(ShowtimeService service)
        {
            showtimeService = service;
        }
        public void SetAuditoriumService(AuditoriumService service)
        {
            auditoriumService = service;
        }

        public async void HandleOnOpen()
        {
            try
            {
                if (!_isInitialized)
                {
                    if (logoutButton != null) logoutButton.Click += HandleLogOutButton;
                    if (homeButton != null) homeButton.Click += (s, e) => screenController.NavigateTo<HomePageUser>();
                    if (bookedTicketButton != null) bookedTicketButton.Click += (s, e) => screenController.NavigateTo<BookedTicket>();
                    if (newsButton != null) newsButton.Click += (s, e) => screenController.NavigateTo<TinTuc>();
                    _isInitialized = true;
                }

                // Check if required services are available
                if (authService == null || movieService == null || screenController == null)
                {
                    MessageBox.Show("Các dịch vụ yêu cầu chưa được khởi tạo", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Try fetch user silently
                User user = null;
                try
                {
                    var result = authService.GetUser();
                    user = result as User;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể lấy thông tin người dùng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Treat as guest or navigate to Home if strict
                    screenController.NavigateTo<Home>();
                    return;
                }

                if (user == null)
                {
                    screenController.NavigateTo<Home>();
                    return;
                }

                UpdateUserUI(user);

                // Clear movie list and show loading
                if (showtimeList != null)
                {
                    showtimeList.Children.Clear();
                    showtimeList.Children.Add(new TextBlock
                    {
                        Text = "Loading showtimes...",
                        Foreground = Brushes.White,
                        FontSize = 16,
                        Margin = new Thickness(10)
                    });
                }

                List<Showtime> showtimes = null;
                string errorMessage = null;
                showtimes = showtimeService.GetAllShowtimes();

                // Clear loading message
                if (showtimeList != null) showtimeList.Children.Clear();

                if (showtimes == null || showtimes.Count == 0)
                {
                    if (showtimeList != null)
                    {
                        var msg = errorMessage ?? "No movies available 🎬";
                        var color = errorMessage != null ? Brushes.Red : Brushes.White;
                        showtimeList.Children.Add(new TextBlock
                        {
                            Text = msg,
                            Foreground = color,
                            FontSize = 16,
                            Margin = new Thickness(10)
                        });
                    }
                    return;
                }

                foreach (var showtime in showtimes)
                {
                    if (showtimeList != null) showtimeList.Children.Add(CreateShowtimeCard(showtime));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong ShowtimePageController: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleSignUpButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<SignUp>();
        }

        public void HandleSignInButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<SignIn>();
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
            if (usernameText == null || logoutButton == null)
            {
                return;
            }

            if (user == null)
            {
                usernameText.Text = "Khách";
                logoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                usernameText.Text = user.Username ?? "Người dùng";
                logoutButton.Visibility = Visibility.Visible;
            }
        }

        private StackPanel CreateShowtimeCard(Showtime showtime)
        {
            var card = new StackPanel
            {
                Width = 220,
                Margin = new Thickness(12),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            // --- Poster ---
            var posterBorder = new Border
            {
                Width = 220,
                Height = 250,
                CornerRadius = new CornerRadius(8),
                ClipToBounds = true,
                Background = Brushes.LightGray
            };

            // Try to load movie poster, fallback to not_found.png if it doesn't exist
            var imageUri = new Uri($"pack://application:,,,/Resources/Images/Movies/{showtime.MovieId}.jpg");

            var poster = new Image
            {
                Width = 220,
                Height = 280,
                Stretch = Stretch.UniformToFill
            };

            try
            {
                poster.Source = new BitmapImage(imageUri);
            }
            catch
            {
                // If movie poster doesn't exist, use not_found.png
                poster.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/Movies/not_found.png"));
                poster.Stretch = Stretch.Uniform;
                poster.Width = 64;
                poster.Height = 64;
                poster.HorizontalAlignment = HorizontalAlignment.Center;
                poster.VerticalAlignment = VerticalAlignment.Center;
            }

            posterBorder.Child = poster;

            // --- Movie name ---
            var title = new TextBlock
            {
                Text = movieService.GetMovieById(showtime.MovieId).Name,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 10, 0, 4),
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Foreground = Brushes.White
            };

            // --- Auditorium name ---
            var auditorium = new TextBlock
            {
                Text = $"Phòng: {auditoriumService.GetAuditoriumById(showtime.AuditoriumId).Name}",
                FontSize = 12,
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 4)
            };

            // --- Time ---
            var timeText = new TextBlock
            {
                Text = $"Giờ chiếu: {showtime.StartTimeString} - {showtime.EndTimeString}",
                FontSize = 12,
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 4)
            };

            // --- Date ---
            var dateText = new TextBlock
            {
                Text = $"Ngày: {showtime.ShowDateString}",
                FontSize = 12,
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 4)
            };

            // --- Price (random) ---
            int[] prices = { 60, 65, 70, 75, 80, 85, 90, 95 };
            int price = prices[_rand.Next(prices.Length)];

            var priceText = new TextBlock
            {
                Text = $"Giá vé: {price}.000 đ",
                FontSize = 12,
                Foreground = Brushes.LightGreen,
                Margin = new Thickness(0, 0, 0, 6)
            };


            // --- Book button ---
            var bookButton = new Button
            {
                Content = "Đặt vé",
                Background = Brushes.DarkRed,
                Foreground = Brushes.White,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(0, 6, 0, 0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            bookButton.Click += (s, e) =>
            {

                // Sau này bạn xử lý chuyển sang màn hình đặt vé
                HandleClickBookTicketButton(showtime.Id, price);
            };

            // Add children to card
            card.Children.Add(posterBorder);
            card.Children.Add(title);
            card.Children.Add(auditorium);
            card.Children.Add(timeText);
            card.Children.Add(dateText);
            card.Children.Add(priceText);
            card.Children.Add(bookButton);

            return card;
        }
        
        public void HandleClickBookTicketButton(Guid showtimeId, int price)
        {
            var controller = screenController.GetController<BookTicketController>();
            if (controller != null)
            {
                controller.SetShowtimeId(showtimeId);
                controller.SetPrice(price);
            }
            screenController.NavigateTo<BookTicket>();
        }
    }
}
