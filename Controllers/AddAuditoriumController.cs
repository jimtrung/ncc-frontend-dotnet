using System;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Utils;
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
        public TextBox auditoriumNameField;
        public TextBox auditoriumTypeField;
        public TextBox auditoriumCapacityField;
        public TextBox auditoriumNoteField;

        public Button backButton;
        public Button addAuditoriumButton;

        private bool _isInitialized = false;

        private void ResetFields()
        {
            if (auditoriumNameField != null)
                auditoriumNameField.Text = string.Empty;

            if (auditoriumTypeField != null)
                auditoriumTypeField.Text = string.Empty;

            if (auditoriumCapacityField != null)
                auditoriumCapacityField.Text = string.Empty;

            if (auditoriumNoteField != null)
                auditoriumNoteField.Text = string.Empty;
        }

        public void HandleOnOpen()
        {
            ResetFields();
            
            if (!_isInitialized)
            {
                if (backButton != null)
                    backButton.Click += (s, e) => HandleBackButton();

                if (addAuditoriumButton != null)
                    addAuditoriumButton.Click += (s, e) => HandleAddAuditoriumButtonClick();

                _isInitialized = true;
            }
        }

        // === Button Handlers ===
        public void HandleBackButton()
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        public void HandleAddAuditoriumButtonClick()
        {

            // Validate empty fields
            if (IsEmpty(auditoriumNameField) ||
                IsEmpty(auditoriumTypeField) ||
                IsEmpty(auditoriumCapacityField))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin",
                                "Lỗi nhập liệu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            // Validate capacity
            if (!int.TryParse(auditoriumCapacityField.Text.Trim(), out int capacity))
            {
                MessageBox.Show("Sức chứa phải là số",
                                "Lỗi nhập liệu",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return;
            }

            // === Create Auditorium (only required fields) ===
            var auditorium = new Auditorium
            {
                Name = auditoriumNameField.Text.Trim(),
                Type = auditoriumTypeField.Text.Trim(),
                Note = auditoriumNoteField?.Text?.Trim(),
                Capacity = capacity
            };

            try
            {
                _auditoriumService.InsertAuditorium(auditorium);

                _auditoriumListController?.RefreshData();

                MessageBox.Show("Thêm phòng chiếu thành công!",
                                "Thành công",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);

                _screenController.NavigateTo<AuditoriumList>();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể thêm phòng chiếu: " + ex.Message,
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        // === Helpers ===
        private bool IsEmpty(TextBox field) => string.IsNullOrWhiteSpace(field?.Text);
    }
}
