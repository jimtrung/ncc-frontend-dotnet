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
                    _isInitialized = true;
                }

                // Check if required services are available
                if (authService == null || movieService == null || screenController == null)
                {
                    System.Diagnostics.Debug.WriteLine("Required services not initialized");
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
                    System.Diagnostics.Debug.WriteLine($"Failed to get user: {ex.Message}");
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

                // List<Movie> movies = null;
                // string errorMessage = null;
                // try
                // {
                //     movies = await movieService.GetAllMoviesAsync();
                // }
                // catch (Exception ex)
                // {
                //     System.Diagnostics.Debug.WriteLine($"Failed to load movies: {ex.Message}");
                //     errorMessage = "Unable to load movies. Please try again later.";
                //     movies = new List<Movie>();
                // }

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
                System.Diagnostics.Debug.WriteLine($"An error occurred in HomePageUser: {ex.Message}");
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

            var imagePath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "Images",
                $"{showtime.MovieId}.jpg"
            );

            var imageUri = System.IO.File.Exists(imagePath)
                ? new Uri(imagePath)
                : new Uri("pack://application:,,,/Resources/Images/cat.jpg");

            var poster = new Image
            {
                Width = 220,
                Height = 280,
                Stretch = Stretch.UniformToFill,
                Source = new BitmapImage(imageUri)
            };

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

        public void BindUIControls(WrapPanel movieList, Button logoutButton, TextBlock usernameText)
        {
            this.showtimeList = movieList;
            this.logoutButton = logoutButton;
            this.usernameText = usernameText;

            if (this.logoutButton != null)
            {
                this.logoutButton.Click -= HandleLogOutButton;
                this.logoutButton.Click += HandleLogOutButton;
            }
        }
    }
}
