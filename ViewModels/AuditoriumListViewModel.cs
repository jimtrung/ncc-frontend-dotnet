using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class AuditoriumListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _auditoriumList;
        public ObservableCollection<string> AuditoriumList
        {
            get => _auditoriumList;
            set { _auditoriumList = value; OnPropertyChanged(); }
        }

        public ICommand CloseCommand { get; private set; }
        public ICommand DeleteAllCommand { get; private set; }
        public ICommand AddAuditoriumCommand { get; private set; }

        public AuditoriumListViewModel()
        {
            AuditoriumList = new ObservableCollection<string>();

            CloseCommand = new RelayCommand(ExecuteClose);
            DeleteAllCommand = new RelayCommand(ExecuteDeleteAll);
            AddAuditoriumCommand = new RelayCommand(ExecuteAddAuditorium);
        }

        private void ExecuteClose(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteDeleteAll(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete ALL auditoriums? This cannot be undone.", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                AuditoriumList.Clear();
                MessageBox.Show("All auditoriums deleted.", "Operation Complete");
            }
        }

        private void ExecuteAddAuditorium(object parameter)
        {
            MessageBox.Show("Open Add Auditorium Window logic goes here.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
