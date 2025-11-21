using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class AuditoriumListController
    {
        private ScreenController _screenController;
        private AuthTokenUtil _authTokenUtil;
        private AuditoriumService _auditoriumService;
        private Guid _selectedAuditoriumId;

        public ObservableCollection<Auditorium> AuditoriumList { get; set; } = new ObservableCollection<Auditorium>();

        public DataGrid auditoriumTable;
        public Button closeBtn;
        public Button addAuditoriumBtn;
        public Button deleteAllBtn;

        public DataGridTextColumn nameColumn;
        public DataGridTextColumn typeColumn;
        public DataGridTextColumn capacityColumn;
        public DataGridTextColumn noteColumn;

        // === Setters ===
        public void SetScreenController(ScreenController screenController) => _screenController = screenController;
        public void SetAuthTokenUtil(AuthTokenUtil tokenUtil) => _authTokenUtil = tokenUtil;
        public void SetAuditoriumService(AuditoriumService service) => _auditoriumService = service;

        // === Initialize / Open ===
        private bool _isInitialized = false;

        // === Initialize / Open ===
        public void HandleOnOpen()
        {
            if (auditoriumTable == null) return;

            if (!_isInitialized)
            {
                if (closeBtn != null) closeBtn.Click += (s, e) => HandleCloseButton();
                if (addAuditoriumBtn != null) addAuditoriumBtn.Click += (s, e) => HandleAddAuditorium();
                if (deleteAllBtn != null) deleteAllBtn.Click += (s, e) => HandleDeleteAllAuditoriums();
                _isInitialized = true;
            }

            // Bind columns
            nameColumn.Binding = new System.Windows.Data.Binding("Name");
            typeColumn.Binding = new System.Windows.Data.Binding("Type");
            capacityColumn.Binding = new System.Windows.Data.Binding("Capacity");
            noteColumn.Binding = new System.Windows.Data.Binding("Note");

            auditoriumTable.ItemsSource = AuditoriumList;

            auditoriumTable.SelectionChanged += (s, e) =>
            {
                if (auditoriumTable.SelectedItem is Auditorium a)
                {
                    _selectedAuditoriumId = a.Id;
                    HandleClickItem(_selectedAuditoriumId);
                }
            };

            RefreshData();
        }

        // === Add Auditorium ===
        public void HandleAddAuditorium()
        {
            var addWindow = new Views.AddAuditorium();
            var controller = addWindow.DataContext as AddAuditoriumController;
            controller.SetScreenController(_screenController);
            controller.SetAuditoriumService(_auditoriumService);
            controller.SetAuthTokenUtil(_authTokenUtil);
            controller.SetAuditoriumListController(this);
            addWindow.ShowDialog();
        }

        // === Click Item ===
        public void HandleClickItem(Guid id)
        {
            var infoWindow = new Views.AuditoriumInformation();
            var controller = infoWindow.DataContext as AuditoriumInformationController;
            controller.SetScreenController(_screenController);
            controller.SetAuditoriumService(_auditoriumService);
            controller.SetAuthTokenUtil(_authTokenUtil);
            controller.SetAuditoriumListController(this);
            controller.SetAuditoriumId(id);
            infoWindow.ShowDialog();
        }

        // === Delete All ===
        public void HandleDeleteAllAuditoriums()
        {
            var result = MessageBox.Show(
                "Are you sure you want to delete all auditoriums?",
                "Delete confirmation",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.OK)
            {
                _auditoriumService.DeleteAllAuditoriums();
                RefreshData();
            }
        }

        // === Close ===
        public void HandleCloseButton()
        {
            _screenController.NavigateTo<HomePageManager>();
        }

        // === Refresh / Update ===
        public void RefreshData()
        {
            if (_auditoriumService != null)
            {
                var list = _auditoriumService.GetAllAuditoriums();
                AuditoriumList.Clear();
                foreach (var a in list)
                    AuditoriumList.Add(a);
            }
        }

        public void UpdateAuditorium(Auditorium updatedAuditorium)
        {
            var existing = AuditoriumList.FirstOrDefault(a => a.Id == updatedAuditorium.Id);
            if (existing != null)
            {
                int idx = AuditoriumList.IndexOf(existing);
                AuditoriumList[idx] = updatedAuditorium;
            }
        }
    }
}
