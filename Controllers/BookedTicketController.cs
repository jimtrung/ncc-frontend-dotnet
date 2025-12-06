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
    public class BookedTicketController
    {
        private ScreenController screenController;
        private AuthService authService;
        private MovieService movieService;
        private AuditoriumService auditoriumService;
        public ShowtimeService showtimeService;
        public TicketService ticketService;
        private AuthTokenUtil authTokenUtil;
        private Guid _uuid;

        private static readonly Random _rand = new Random();

        public WrapPanel bookedTicketList;
        public Button logoutButton;
        public Button homeButton;
        public Button bookedTicketButton;
        public Button showTimesButton;
        public TextBlock usernameText;
        public Button deleteAllButton;
        public Button newsButton;
        public Button promotionButton;
        public Button priceButton;
        public Button aboutButton;

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
        public void SetTicketService(TicketService service)
        {
            ticketService = service;
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
                    if (showTimesButton != null) showTimesButton.Click += (s, e) => screenController.NavigateTo<ShowtimePage>();
                    if (deleteAllButton != null) deleteAllButton.Click += (s, e) => DeleteAllButton();
                    
                    if (newsButton != null) newsButton.Click += (s, e) => screenController.NavigateTo<TinTuc>();
                    if (promotionButton != null) promotionButton.Click += (s, e) => screenController.NavigateTo<EventList>();
                    _isInitialized = true;
                }

                // Check if required services are available
                if (authService == null || movieService == null || screenController == null || ticketService == null || authTokenUtil == null || showtimeService == null || auditoriumService == null)
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
                if (bookedTicketList != null)
                {
                    bookedTicketList.Children.Clear();
                        bookedTicketList.Children.Add(new TextBlock
                        {
                            Text = "Đang tải vé đã đặt...",
                            Foreground = Brushes.White,
                            FontSize = 16,
                            Margin = new Thickness(10)
                        });
                    }
                    var token = authTokenUtil.LoadAccessToken();
                    var userId = JwtUtil.GetUserIdFromToken(token);

                    List<Ticket> tickets = null;
                    string errorMessage = null;
                    // MessageBox.Show($"User ID: {userId}");
                    tickets = ticketService.GetTicketsByUserId(userId.Value);


                    // Clear loading message
                    if (bookedTicketList != null) bookedTicketList.Children.Clear();

                    if (tickets == null || tickets.Count == 0)
                    {
                        if (bookedTicketList != null)
                        {
                            var msg = errorMessage ?? "Bạn chưa có vé đã đặt !";
                            var color = errorMessage != null ? Brushes.Red : Brushes.White;
                            bookedTicketList.Children.Add(new TextBlock
                            {
                                Text = msg,
                                Foreground = color,
                                FontSize = 16,
                                Margin = new Thickness(10)
                            });
                        }
                        return;
                    }

                foreach (var ticket in tickets)
                {
                    if (bookedTicketList != null) bookedTicketList.Children.Add(CreateBookedTicketCard(ticket));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong BookedTicketController: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private StackPanel CreateBookedTicketCard(Ticket bookedTicket)
        {
            var card = new StackPanel
            {
                Width = 220,
                Margin = new Thickness(12),
                Background = new SolidColorBrush(Color.FromRgb(30, 30, 30)),
                Cursor = System.Windows.Input.Cursors.Arrow
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

            var showtime = showtimeService.GetShowtimeById(bookedTicket.Showtimeid);
            var movie = movieService.GetMovieById(showtime.MovieId);
            var auditorium = auditoriumService.GetAuditoriumById(showtime.AuditoriumId);

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
                Text = movie.Name,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 10, 0, 4),
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Foreground = Brushes.White
            };

            // --- Auditorium ---
            var auditorium_ = new TextBlock
            {
                Text = $"Phòng: {auditorium.Name}",
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

            // --- Price & Seat ---
            var priceSeatPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 6)
            };

            var priceText = new TextBlock
            {
                Text = $"Giá vé: {bookedTicket.Price}.000 đ",
                FontSize = 12,
                Foreground = Brushes.LightGreen,
                VerticalAlignment = VerticalAlignment.Center
            };

            var seatText = new TextBlock
            {
                Text = $"Ghế: {bookedTicket.Seatname}",
                FontSize = 12,
                Foreground = Brushes.LightGray,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(20, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            priceSeatPanel.Children.Add(priceText);
            priceSeatPanel.Children.Add(seatText);

            // --- Buttons panel ---
            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 6, 0, 0)
            };

            var deleteButton = new Button
            {
                Content = "Xóa",
                Background = Brushes.Gray,
                Foreground = Brushes.White,
                Padding = new Thickness(8, 4, 8, 4),
                Margin = new Thickness(0, 0, 6, 0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            deleteButton.Click += (s, e) =>
            {
                DeleteTicketAndRefresh(bookedTicket.Id);
            };

            var payButton = new Button
            {
                Content = "Thanh toán",
                Background = Brushes.DarkGreen,
                Foreground = Brushes.White,
                Padding = new Thickness(8, 4, 8, 4),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            payButton.Click += (s, e) =>
            {
                HandlePayBookedTicket(bookedTicket.Id);
            };

            buttonsPanel.Children.Add(deleteButton);
            buttonsPanel.Children.Add(payButton);

            // Add children to card
            card.Children.Add(posterBorder);
            card.Children.Add(title);
            card.Children.Add(auditorium_);
            card.Children.Add(timeText);
            card.Children.Add(dateText);
            card.Children.Add(priceSeatPanel);
            card.Children.Add(buttonsPanel);

            return card;
        }

        private void DeleteTicketAndRefresh(Guid ticketId)
        {
            try
            {
                // Gọi xóa vé
                ticketService.DeleteTicketById(ticketId);

                // Reload danh sách vé
                HandleOnOpen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xảy ra lỗi khi xóa vé: {ex.Message}");
            }
        }

        private void DeleteAllButton()
        {
            try
            {
                // Hiện hộp xác nhận
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa tất cả các vé đã đặt không?",
                    "Xác nhận xóa tất cả",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (result != MessageBoxResult.Yes)
                {
                    // Người dùng hủy, không xóa
                    return;
                }

                // Gọi xóa tất cả vé
                ticketService.DeleteAllTickets();

                // Reload danh sách vé
                HandleOnOpen();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Xảy ra lỗi khi xóa vé: {ex.Message}");
            }
        }


        public void HandlePayBookedTicket(Guid bookedTicketId)
        {
            // Implement payment logic here
            var controller = screenController.GetController<PayPageController>();
            if (controller != null)
            {
                controller.SetTicketId(bookedTicketId);
                screenController.NavigateTo<PayPage>();
            }
        }


    }
}
