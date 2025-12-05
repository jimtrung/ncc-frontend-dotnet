using System;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Views;


namespace Theater_Management_FE.Controllers
{
    public class PayPageController
    {
        private TicketService _ticketService;
        private ShowtimeService _showtimeService;
        private MovieService _movieService;
        private AuditoriumService _auditoriumService;
        private ScreenController _screenController;

        public void SetTicketService(TicketService service) => _ticketService = service;
        public void SetShowtimeService(ShowtimeService service) => _showtimeService = service;
        public void SetMovieService(MovieService service) => _movieService = service;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetTicketId(Guid ticketId) => _ticketId = ticketId;

        // Controls in the TicketCard UserControl
        public TextBlock ticketMovieName;
        public TextBlock ticketAuditoriumName;
        public TextBlock ticketStartTime;
        public TextBlock ticketEndTime;
        public TextBlock ticketShowDate;
        public TextBlock ticketSeatName;
        public TextBlock ticketPrice;
        public Image ticketQRCode;
        public Button back;

        private Guid _ticketId;

        private bool _isInitialized = false;

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (back != null) back.Click += (s, e) => _screenController.NavigateTo<BookedTicket>();
                _isInitialized = true;
            }

            try
            {
                var ticket = _ticketService.GetTicketById(_ticketId);
                var showtime = _showtimeService.GetShowtimeById(ticket.Showtimeid);
                var movie = _movieService.GetMovieById(showtime.MovieId);
                var auditorium = _auditoriumService.GetAuditoriumById(showtime.AuditoriumId);

                ticketMovieName.Text = movie.Name;
                ticketAuditoriumName.Text = auditorium.Name;
                ticketStartTime.Text = showtime.StartTime.ToString("HH:mm");
                ticketEndTime.Text = showtime.EndTime.ToString("HH:mm");
                ticketShowDate.Text = showtime.ShowDate.ToString("dd/MM/yyyy");
                ticketSeatName.Text = ticket.Seatname;
                // Nhân 1000 để hiển thị
                ticketPrice.Text = (ticket.Price * 1000).ToString("N0", new System.Globalization.CultureInfo("vi-VN"));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ticket information: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
