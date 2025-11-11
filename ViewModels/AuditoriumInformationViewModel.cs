using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class AuditoriumInformationViewModel : INotifyPropertyChanged
    {
        private string _auditoriumName;
        private string _auditoriumType;
        private string _auditoriumCapacity;
        private string _auditoriumNote;

        public string AuditoriumName
        {
            get => _auditoriumName;
            set { _auditoriumName = value; OnPropertyChanged(); }
        }

        public string AuditoriumType
        {
            get => _auditoriumType;
            set { _auditoriumType = value; OnPropertyChanged(); }
        }

        public string AuditoriumCapacity
        {
            get => _auditoriumCapacity;
            set { _auditoriumCapacity = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanExecuteEdit)); }
        }

        public string AuditoriumNote
        {
            get => _auditoriumNote;
            set { _auditoriumNote = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public AuditoriumInformationViewModel()
        {
            AuditoriumName = string.Empty;
            AuditoriumType = string.Empty;
            AuditoriumCapacity = string.Empty;
            AuditoriumNote = string.Empty;

            BackCommand = new RelayCommand(ExecuteBack);
            EditCommand = new RelayCommand(ExecuteEdit, CanExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private bool CanExecuteEdit(object parameter)
        {
            return int.TryParse(AuditoriumCapacity, out _);
        }

        private void ExecuteEdit(object parameter)
        {
            string message = $"Editing Auditorium:\nName: {AuditoriumName}\nType: {AuditoriumType}\nCapacity: {AuditoriumCapacity}\nNote: {AuditoriumNote}";
            MessageBox.Show(message, "Edit Successful");
        }

        private void ExecuteDelete(object parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this auditorium?", "Confirm Deletion", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Auditorium deleted.", "Deletion Successful");
                if (parameter is Window window)
                {
                    window.Close();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
