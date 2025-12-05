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
using System.Reflection.Metadata;
using Newtonsoft.Json;


namespace Theater_Management_FE.Controllers
{
    public class BookTicketController
    {
        private ScreenController screenController;
        private AuthService authService;
        private MovieService movieService;
        private AuditoriumService auditoriumService;
        public ShowtimeService showtimeService;

        public TicketService ticketService;
        private AuthTokenUtil authTokenUtil;

        private Guid _uuid;
        private int _price;

        public TextBox ticketMovieName;
        public TextBox ticketAuditoriumName;
        public TextBox ticketDate;
        public TextBox ticketStartTime;
        public TextBox ticketEndTime;
        public TextBox ticketPrice;
        public Button backButton;
        public Button bookTicketButton;
        public Button deleteAllButton;
        public ComboBox ticketSeatSelector;
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
        public void SetShowtimeId(Guid id)
        {
            _uuid = id;
        }
        public void SetPrice(int price)
        {
            _price = price;
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
                    _isInitialized = true;
                    if (backButton != null) backButton.Click += (s, e) => screenController.NavigateTo<ShowtimePage>();
                    if (bookTicketButton != null) bookTicketButton.Click += (s, e) => HandleBookTicketButton();
                    
                    // Debug logs
                    MessageBox.Show($"[BookTicketController] Initialized. bookTicketButton is {(bookTicketButton == null ? "NULL" : "OK")}");
                }

                // Gọi API lấy showtime
                var showtime = showtimeService.GetShowtimeById(_uuid);

                if (showtime != null)
                {
                    ticketMovieName.Text = movieService.GetMovieById(showtime.MovieId).Name;
                    ticketAuditoriumName.Text = auditoriumService.GetAuditoriumById(showtime.AuditoriumId).Name;
                    ticketDate.Text = showtime.ShowDate.ToString("yyyy-MM-dd");
                    ticketStartTime.Text = showtime.StartTime.ToString("HH:mm");
                    ticketEndTime.Text = showtime.EndTime.ToString("HH:mm");

                    // Giá đã set từ SetPrice()
                    ticketPrice.Text = $"{_price}.000 đ";
                }
                else
                {
                    MessageBox.Show($"Lỗi: Không tìm thấy suất chiếu với ID: {_uuid}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi trong BookTicketController: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async void HandleBookTicketButton()
        {
            try
            {
                // 1. Lấy ghế được chọn
                var selectedSeat = (ticketSeatSelector.SelectedItem as ComboBoxItem)?.Content?.ToString();

                if (string.IsNullOrEmpty(selectedSeat))
                {
                    MessageBox.Show("Vui lòng chọn ghế!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var token = authTokenUtil.LoadAccessToken();
                var userId = JwtUtil.GetUserIdFromToken(token);

                if (userId == null)
                {
                    MessageBox.Show("Không tìm thấy userId trong token!", "Lỗi");
                    return;
                }

                if (ticketSeatSelector == null) MessageBox.Show("Lỗi: ticketSeatSelector bị null", "Lỗi");
                if (authTokenUtil == null) MessageBox.Show("Lỗi: authTokenUtil bị null", "Lỗi");
                if (ticketService == null) MessageBox.Show("Lỗi: ticketService bị null", "Lỗi");
                if (screenController == null) MessageBox.Show("Lỗi: screenController bị null", "Lỗi");


                // 3. Tạo object Ticket
                var ticket = new Ticket
                {
                    Id = Guid.NewGuid(),
                    Userid = userId.Value,
                    Showtimeid = _uuid,
                    Seatname = selectedSeat,
                    Price = _price,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // 4. Gọi API
                await ticketService.InsertTicket(ticket);

                // 5. Success
                MessageBox.Show("Đặt vé thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);

                // 6. Quay lại danh sách showtime
                screenController.NavigateTo<ShowtimePage>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đặt vé thất bại: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
