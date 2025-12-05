using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class HomePageManagerController
    {
        public Grid chartGrid;

        public Button movieButton;
        public Button profileButton;
        public Button auditoriumButton;
        public Button logOutButton;
        public Button showtimeButton;

        private bool _isInitialized = false;

        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private AuditoriumService _auditoriumService;
        private ShowtimeService _showtimeService;
        private TicketService _ticketService;
        public TextBlock userCountText;
        public TextBlock movieCountText;
        public TextBlock auditoriumCountText;
        public TextBlock showtimeCountText;
        public TextBlock ticketCountText;
        public TextBlock revenueText;
        public UserService _userService;


        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthTokenUtil(AuthTokenUtil authTokenUtil)
        {
            _authTokenUtil = authTokenUtil;
        }
        public void SetMovieService(MovieService movieService)
        {
            _movieService = movieService;
        }
        public void SetAuditoriumService(AuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
        }
        public void SetShowtimeService(ShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }
        public void SetTicketService(TicketService ticketService)
        {
            _ticketService = ticketService;
        }
        public void SetUserService(UserService userService)
        {
            _userService = userService;
        }

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (movieButton != null) movieButton.Click += HandleMovieButton;
                if (auditoriumButton != null) auditoriumButton.Click += HandleAuditoriumButton;
                if (profileButton != null) profileButton.Click += HandleProfileButton;
                if (logOutButton != null) logOutButton.Click += HandleLogOutButton;
                if (showtimeButton != null) showtimeButton.Click += HandleShowtimeButton;
                
                if (chartGrid != null) DrawColumnChart();

                _isInitialized = true;
            }

            var movies = _movieService.GetAllMovies(); int movieCount = movies != null ? movies.Count : 0;
            var auditoriums = _auditoriumService.GetAllAuditoriums(); int auditoriumCount = auditoriums != null ? auditoriums.Count : 0;
            var showtimes = _showtimeService.GetAllShowtimes(); int showtimeCount = showtimes != null ? showtimes.Count : 0;
            var tickets = _ticketService.GetAllTickets(); int ticketCount = tickets != null ? tickets.Count : 0;
            var revenue = 0;

            foreach (var ticket in tickets)
            {
                revenue += ticket.Price;
            }

            var users = _userService.GetAllUsers();
            // Gán số liệu vào TextBlock
            if (userCountText != null) userCountText.Text = $"- Người dùng: {users}";
            if (movieCountText != null) movieCountText.Text = $"- Phim: {movieCount}";
            if (auditoriumCountText != null) auditoriumCountText.Text = $"- Phòng: {auditoriumCount}";
            if (showtimeCountText != null) showtimeCountText.Text = $"- Suất chiếu: {showtimeCount}";
            if (ticketCountText != null) ticketCountText.Text = $"- Vé: {ticketCount}";
            if (revenueText != null) revenueText.Text = $"- Tổng doanh thu dự kiến: {revenue}.000 VNĐ";
        }



        private void DrawColumnChart()
        {
            var movies = _movieService.GetAllMovies(); int movieCount = movies != null ? movies.Count : 0;
            var auditoriums = _auditoriumService.GetAllAuditoriums(); int auditoriumCount = auditoriums != null ? auditoriums.Count : 0;
            var showtimes = _showtimeService.GetAllShowtimes(); int showtimeCount = showtimes != null ? showtimes.Count : 0;
            var tickets = _ticketService.GetAllTickets(); int ticketCount = tickets != null ? tickets.Count : 0;
            var users = _userService.GetAllUsers();

            string[] labels = { "Người dùng", "Phim", "Phòng", "Suất chiếu", "Vé" };
            double[] values = { users, movieCount, auditoriumCount, showtimeCount, ticketCount };
            double maxVal = 0;

            foreach (var v in values)
                if (v > maxVal) maxVal = v;

            double columnWidth = 50;
            double spacing = 40;
            double chartHeight = 300;
            double paddingLeft = 20;  // padding trái
            double paddingRight = 20; // padding phải

            chartGrid.Height = chartHeight + 60; // thêm margin trên + dưới
            var canvas = new Canvas { Height = chartHeight + 60 };
            chartGrid.Children.Add(canvas);

            for (int i = 0; i < labels.Length; i++)
            {
                double heightPercent = values[i] / maxVal;
                double columnHeight = chartHeight * heightPercent;

                // Cột
                var rect = new Rectangle
                {
                    Width = columnWidth,
                    Height = columnHeight,
                    Fill = Brushes.CornflowerBlue,
                    RadiusX = 4,
                    RadiusY = 4
                };
                Canvas.SetLeft(rect, paddingLeft + i * (columnWidth + spacing));
                Canvas.SetBottom(rect, 30); // cách đáy một chút
                canvas.Children.Add(rect);

                // Giá trị trên cột
                var valText = new TextBlock
                {
                    Text = values[i].ToString(),
                    Foreground = Brushes.White,
                    // FontWeight = FontWeights.Bold,
                    FontSize = 16 // chữ số lớn hơn
                };
                Canvas.SetLeft(valText, paddingLeft + i * (columnWidth + spacing) + 5);
                Canvas.SetBottom(valText, columnHeight + 35); // cách cột một chút
                canvas.Children.Add(valText);

                // Label dưới cột
                var lbl = new TextBlock
                {
                    Text = labels[i],
                    Foreground = Brushes.White,
                    Width = columnWidth + 20,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 14,  // chữ tên cột lớn hơn
                    // FontWeight = FontWeights.SemiBold
                };
                Canvas.SetLeft(lbl, paddingLeft + i * (columnWidth + spacing) - 10);
                Canvas.SetBottom(lbl, 5); // cách đáy một chút
                canvas.Children.Add(lbl);
            }
        }
        private void HandleMovieButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<MovieList>();
        }
        private void HandleAuditoriumButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        private void HandleProfileButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<Profile>();
        }

        private void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            _authTokenUtil.ClearRefreshToken();
            _authTokenUtil.ClearAccessToken();
            _screenController.NavigateTo<Home>();
        }

        private void HandleShowtimeButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<ShowtimeList>();
        }
    }
}
