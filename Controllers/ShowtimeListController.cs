using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class ShowtimeListController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private ShowtimeService _showtimeService;
        private MovieService _movieService;
        private AuditoriumService _auditoriumService;
        private Guid _uuid;

        public ObservableCollection<Showtime> ShowtimeList { get; set; } = new ObservableCollection<Showtime>();

        public DataGrid showtimeTable;
        public Button closeBtn;
        public Button addShowtimeBtn;
        public Button deleteAllBtn;

        public DataGridTextColumn movieColumn;
        public DataGridTextColumn auditoriumColumn;
        public DataGridTextColumn startColumn;
        public DataGridTextColumn finishColumn;
        public DataGridTextColumn dateColumn;
        public DataGridTextColumn quantityColumn;

        public void SetScreenController(ScreenController screenController) => _screenController = screenController;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetShowtimeService(ShowtimeService showtimeService) => _showtimeService = showtimeService;

        public void SetMovieService(MovieService movieService) => _movieService = movieService;
        public void SetAuditoriumService(AuditoriumService auditoriumService) => _auditoriumService = auditoriumService;

        private bool _isInitialized = false;

        public void HandleOnOpen()
        {
            if (showtimeTable == null) return;

            if (!_isInitialized)
            {
                if (closeBtn != null) closeBtn.Click += (s, e) => HandleCloseBtn();
                if (addShowtimeBtn != null) addShowtimeBtn.Click += (s, e) => HandleAddShowtime();
                if (deleteAllBtn != null) deleteAllBtn.Click += (s, e) => HandleDeleteAllShowtime();
                _isInitialized = true;
            }

            movieColumn.Binding = new System.Windows.Data.Binding("MovieName");
            auditoriumColumn.Binding = new System.Windows.Data.Binding("AuditoriumName");
            startColumn.Binding = new System.Windows.Data.Binding("StartTimeString");
            finishColumn.Binding = new System.Windows.Data.Binding("EndTimeString");
            dateColumn.Binding = new System.Windows.Data.Binding("ShowDateString");
            quantityColumn.Binding = new System.Windows.Data.Binding("Quantity");

            showtimeTable.ItemsSource = ShowtimeList;

            showtimeTable.SelectionChanged += (s, e) =>
            {
                if (showtimeTable.SelectedItem is Showtime selectedShowtime)
                {
                    _uuid = selectedShowtime.Id;
                    HandleClickItem(_uuid);

                }
            };
            // Sau đó load dữ liệu
            LoadShowtimes();
        }

        public void HandleClickItem(Guid id)
        {
            try
            {
                var showtimeInfoController = _screenController.GetController<ShowtimeInformationController>();
                if (showtimeInfoController != null)
                {
                    showtimeInfoController.SetShowtimeId(id);
                    _screenController.NavigateTo<ShowtimeInformation>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể điều hướng đến chi tiết suất chiếu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void HandleAddShowtime()
        {
            _screenController.NavigateTo<AddShowtime>();
        }

        public void HandleCloseBtn()
        {
            _screenController.NavigateTo<HomePageManager>();
        }

        public void HandleDeleteAllShowtime()
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả suất chiếu không?", "Xác nhận xóa", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _showtimeService.DeleteAllShowtimes();
                LoadShowtimes();
            }
        }

        public void LoadShowtimes()
        {
            if (_showtimeService == null || _movieService == null || _auditoriumService == null) return;

            // Lấy tất cả showtime từ service
            var showtimes = _showtimeService.GetAllShowtimes();

            ShowtimeList.Clear();

            foreach (var s in showtimes)
            {
                // Lấy Movie và Auditorium tương ứng
                var movie = _movieService.GetMovieById(s.MovieId);
                var auditorium = _auditoriumService.GetAuditoriumById(s.AuditoriumId);

                // Tạo object mới với dữ liệu để binding DataGrid
                var showtimeItem = new Showtime
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    AuditoriumId = s.AuditoriumId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    ShowDate = s.ShowDate,

                    // Có thể dùng các property tạm để binding tên phim, phòng
                    MovieName = movie?.Name ?? "Không xác định",
                    AuditoriumName = auditorium?.Name ?? "Không xác định",
                    Quantity = auditorium != null ? auditorium.Capacity : 0 // gán capacity
                };

                ShowtimeList.Add(showtimeItem);
            }
        }

    }
}
