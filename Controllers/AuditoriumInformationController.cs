using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

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

        public Button BackButton;
        public Button EditButton;
        public Button DeleteButton;

        public void SetScreenController(ScreenController controller) => _screenController = controller;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetAuditoriumListController(AuditoriumListController controller) => _auditoriumListController = controller;
        public void SetAuditoriumId(Guid id) => _auditoriumId = id;

        public void HandleOnOpen()
        {
            var auditorium = _auditoriumService.GetAuditoriumById(_auditoriumId);
            if (auditorium == null) return;

            AuditoriumCapacityField.Text = auditorium.Capacity.ToString();
        }

        public void HandleBackButton()
        {
            _screenController.NavigateTo<AuditoriumList>();
        }

        public void HandleEditButton()
        {
            try
            {
                var updatedAuditorium = new Auditorium
                {

                };

                if (int.TryParse(AuditoriumCapacityField.Text.Trim(), out int capacity))
                    updatedAuditorium.Capacity = capacity;
                else
                {
                    updatedAuditorium.Capacity = 0;
                }

                _auditoriumService.UpdateAuditorium(_auditoriumId, updatedAuditorium);
                var auditorium = _auditoriumService.GetAuditoriumById(_auditoriumId);
                _auditoriumListController.UpdateAuditorium(auditorium);

                _screenController.NavigateTo<AuditoriumList>();
            }
            catch (Exception)
            {
            }
        }

        public void HandleDeleteButton()
        {
            _auditoriumService.DeleteAuditoriumById(_auditoriumId);
            _auditoriumListController.RefreshData();
            _screenController.NavigateTo<AuditoriumList>();
        }

        public void BindUIControls(TextBox name, TextBox type, TextBox capacity, TextBox note, Button back, Button edit, Button delete)
        {
            AuditoriumNameField = name;
            AuditoriumTypeField = type;
            AuditoriumCapacityField = capacity;
            AuditoriumNoteField = note;
            BackButton = back;
            EditButton = edit;
            DeleteButton = delete;

            BackButton.Click += (s, e) => HandleBackButton();
            EditButton.Click += (s, e) => HandleEditButton();
            DeleteButton.Click += (s, e) => HandleDeleteButton();
        }
    }
}
