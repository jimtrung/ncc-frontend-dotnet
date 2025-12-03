using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;
using System.Reflection.Metadata.Ecma335;

namespace Theater_Management_FE.Controllers
{
    public class AddShowtimeController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private MovieService _movieService;
        private AuditoriumService _auditoriumService;
        private ShowtimeListController _showtimeListController;
        private ShowtimeService _showtimeService;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthTokenUtil(AuthTokenUtil util) => _authTokenUtil = util;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetShowtimeListController(ShowtimeListController controller) => _showtimeListController = controller;
        public void SetShowtimeService(ShowtimeService service) => _showtimeService = service;

        // UI Controls
        public ListView movieListView;
        public ListView auditoriumListView;
        public TextBox searchMovieField;
        public TextBox searchAuditoriumField;
        public Button backButton;
        public Button addButton;
        public ComboBox startTimePicker;
        public ComboBox endTimePicker;
        public DatePicker showtimeDatePicker;

        private bool _isInitialized = false;

        // Dữ liệu
        private ObservableCollection<SelectableItem<Movie>> _movies = new();
        private ObservableCollection<SelectableItem<Auditorium>> _auditoriums = new();

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += (s, e) => HandleBackButton();
                if (addButton != null) addButton.Click += (s, e) => HandleAddShowtimeButtonClick();
                _isInitialized = true;
            }

            LoadList(movieListView, searchMovieField, _movies, _movieService?.GetAllMovies());
            LoadList(auditoriumListView, searchAuditoriumField, _auditoriums, _auditoriumService?.GetAllAuditoriums());
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<ShowtimeList>();
        }

        public void HandleAddShowtimeButtonClick()
        {
            if (_showtimeService == null) return;

            // Lấy phim được chọn
            var selectedMovie = _movies.FirstOrDefault(x => x.IsSelected)?.Item;
            if (selectedMovie == null)
            {
                MessageBox.Show("Please select a movie.");
                return;
            }

            // Lấy phòng được chọn
            var selectedAuditorium = _auditoriums.FirstOrDefault(x => x.IsSelected)?.Item;
            if (selectedAuditorium == null)
            {
                MessageBox.Show("Please select an auditorium.");
                return;
            }

            // Lấy thời gian từ ComboBox
            if (startTimePicker.SelectedItem is not ComboBoxItem startItem ||
                endTimePicker.SelectedItem is not ComboBoxItem endItem)
            {
                MessageBox.Show("Please select start and end time.");
                return;
            }

            if (!TimeSpan.TryParse(startItem.Content.ToString(), out var startTimeSpan) ||
                !TimeSpan.TryParse(endItem.Content.ToString(), out var endTimeSpan))
            {
                MessageBox.Show("Invalid time format.");
                return;
            }

            // Sử dụng ngày hiện tại để kết hợp với giờ (không dùng DatePicker)
            var today = DateTime.Today;
            var startDateTimeLocal = today.Add(startTimeSpan);
            var endDateTimeLocal = today.Add(endTimeSpan);

            // Kiểm tra hợp lệ
            if (endDateTimeLocal <= startDateTimeLocal)
            {
                MessageBox.Show("End time must be after start time.");
                return;
            }

            // Giới hạn tối đa 3 giờ
            var maxEndTime = startDateTimeLocal.AddHours(3);
            if (endDateTimeLocal > maxEndTime)
            {
                endDateTimeLocal = maxEndTime;
                MessageBox.Show("End time limited to maximum 3 hours after start time.");
                return;
            }

            // Chuyển sang UTC trước khi lưu DB
            var startDateTimeUtc = startDateTimeLocal.AddHours(-10).ToUniversalTime();
            var endDateTimeUtc = endDateTimeLocal.AddHours(-10).ToUniversalTime();

            // Lấy ngày từ DatePicker
            // var selectedDate = showtimeDatePicker.SelectedDate; // Nullable<DateTime>
            var dateOnly = showtimeDatePicker.SelectedDate.Value.Date;
            var dateOnlyUtc = DateTime.SpecifyKind(dateOnly, DateTimeKind.Utc);

            // Tạo Showtime
            var showtime = new Showtime
            {
                Id = Guid.NewGuid(),
                MovieId = selectedMovie.Id,
                AuditoriumId = selectedAuditorium.Id,
                StartTime = startDateTimeUtc,
                EndTime = endDateTimeUtc,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ShowDate = dateOnlyUtc
            };

            _showtimeService.InsertShowtime(showtime);

            MessageBox.Show("Showtime added successfully!");
            _screenController.NavigateTo<ShowtimeList>();
        }

        private void LoadList<T>(ListView listView, TextBox searchField,
            ObservableCollection<SelectableItem<T>> collection, IEnumerable<T> items)
            where T : class
        {
            if (listView == null || items == null || collection == null) return;

            collection.Clear();
            foreach (var item in items)
                collection.Add(new SelectableItem<T> { Item = item, IsSelected = false });

            listView.ItemsSource = collection;

            if (searchField != null)
            {
                searchField.TextChanged += (s, e) =>
                {
                    string filter = searchField.Text.Trim().ToLower();
                    listView.ItemsSource = string.IsNullOrEmpty(filter)
                        ? collection
                        : new ObservableCollection<SelectableItem<T>>(
                            collection.Where(x => x.Item.GetType()
                                .GetProperty("Name")?
                                .GetValue(x.Item)?.ToString()
                                .ToLower().Contains(filter) ?? false)
                        );
                };
            }
        }

        public class SelectableItem<T> : INotifyPropertyChanged
        {
            private bool _isSelected;
            public T Item { get; set; }
            public bool IsSelected
            {
                get => _isSelected;
                set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
