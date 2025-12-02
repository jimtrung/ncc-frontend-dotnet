using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;
using System.Windows;


namespace Theater_Management_FE.Controllers
{
    public class AuditoriumInformationController
    {
        private ScreenController _screenController;
        private AuditoriumService _auditoriumService;
        private AuthTokenUtil _authTokenUtil;
        private AuditoriumListController _auditoriumListController;
        private Guid _auditoriumId;

        public TextBox AuditoriumNameField;
        public TextBox AuditoriumTypeField;
        public TextBox AuditoriumCapacityField;
        public TextBox AuditoriumNoteField;

        public Button backButton;
        public Button editButton;
        public Button deleteButton;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetAuditoriumListController(AuditoriumListController controller) => _auditoriumListController = controller;
        public void SetAuditoriumId(Guid id) => _auditoriumId = id;

        private bool _isInitialized = false;
        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (backButton != null) backButton.Click += (s, e) => HandleBackButton();
                if (editButton != null) editButton.Click += (s, e) => HandleEditButton();
                if (deleteButton != null) deleteButton.Click += (s, e) => HandleDeleteButton();
                _isInitialized = true;
            }
            var auditorium = _auditoriumService.GetAuditoriumById(_auditoriumId);
            if (auditorium == null) return;

            AuditoriumNameField.Text = auditorium.Name;
            AuditoriumTypeField.Text = auditorium.Type;
            AuditoriumCapacityField.Text = auditorium.Capacity.ToString();
            AuditoriumNoteField.Text = auditorium.Note;
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        public void HandleEditButton()
        {
            if (string.IsNullOrWhiteSpace(AuditoriumNameField.Text) ||
                string.IsNullOrWhiteSpace(AuditoriumTypeField.Text) ||
                string.IsNullOrWhiteSpace(AuditoriumCapacityField.Text))
            {
                MessageBox.Show("Please fill in all required fields.",
                                "Input Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(AuditoriumCapacityField.Text.Trim(), out int capacity))
            {
                MessageBox.Show("Capacity must be a number.",
                                "Input Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            var updatedAuditorium = new Auditorium
            {
                Name = AuditoriumNameField.Text.Trim(),
                Type = AuditoriumTypeField.Text.Trim(),
                Capacity = capacity,
                Note = AuditoriumNoteField.Text?.Trim()
            };

            _auditoriumService.UpdateAuditorium(_auditoriumId, updatedAuditorium);
            MessageBox.Show("Auditorium updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            var updated = _auditoriumService.GetAuditoriumById(_auditoriumId);
            _auditoriumListController.UpdateAuditorium(updated);

            _screenController.NavigateTo<AuditoriumList>();
        }


        public void HandleDeleteButton()
        {
            var result = MessageBox.Show("Are you sure you want to delete this auditorium?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _auditoriumService.DeleteAuditoriumById(_auditoriumId);
                _auditoriumListController.RefreshData();
                _screenController.NavigateTo<AuditoriumList>();
            }
        }

        public void BindUIControls(TextBox name, TextBox type, TextBox capacity, TextBox note, Button back, Button edit, Button delete)
        {
            AuditoriumNameField = name;
            AuditoriumTypeField = type;
            AuditoriumCapacityField = capacity;
            AuditoriumNoteField = note;
            backButton = back;
            editButton = edit;
            deleteButton = delete;

            backButton.Click += (s, e) => HandleBackButton();
            editButton.Click += (s, e) => HandleEditButton();
            deleteButton.Click += (s, e) => HandleDeleteButton();
        }
    }
}
