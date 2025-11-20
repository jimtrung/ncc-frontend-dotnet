using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class AddAuditoriumController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private AuditoriumService _auditoriumService;
        private AuditoriumListController _auditoriumListController;

        // === Setters ===
        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetAuditoriumListController(AuditoriumListController controller) => _auditoriumListController = controller;

        // === Fields from XAML ===
        public TextBox AuditoriumNameField;
        public TextBox AuditoriumTypeField;
        public TextBox AuditoriumCapacityField;
        public TextBox AuditoriumNoteField;

        public Button BackButton;
        public Button AddAuditoriumButton;

        // === Button Handlers ===
        public void HandleBackButton()
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        public void HandleAddAuditoriumButtonClick()
        {
            if (IsEmpty(AuditoriumNameField) ||
                IsEmpty(AuditoriumTypeField) ||
                IsEmpty(AuditoriumCapacityField))
            {
                MessageBox.Show("Please enter complete information", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(AuditoriumCapacityField.Text.Trim(), out int capacity))
            {
                MessageBox.Show("Capacity must be a number", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var auditorium = new Auditorium
            {
                Capacity = capacity,
            };

            try
            {
                _auditoriumService.InsertAuditorium(auditorium);
                _auditoriumListController.RefreshData();
                _screenController.NavigateTo<AuditoriumList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add auditorium: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // === Helpers ===
        private bool IsEmpty(TextBox field) => string.IsNullOrWhiteSpace(field?.Text);

        public void BindUIControls(TextBox name, TextBox type, TextBox capacity, TextBox note, Button back, Button add)
        {
            AuditoriumNameField = name;
            AuditoriumTypeField = type;
            AuditoriumCapacityField = capacity;
            AuditoriumNoteField = note;
            BackButton = back;
            AddAuditoriumButton = add;

            BackButton.Click += (s, e) => HandleBackButton();
            AddAuditoriumButton.Click += (s, e) => HandleAddAuditoriumButtonClick();
        }
    }
}
