using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class AddAuditoriumViewModel : INotifyPropertyChanged
    {
        private string _auditoriumName;
        public string AuditoriumName
        {
            get => _auditoriumName;
            set
            {
                _auditoriumName = value;
                OnPropertyChanged();
                ((RelayCommand)AddAuditoriumCommand).RaiseCanExecuteChanged();
            }
        }

        private string _auditoriumType;
        public string AuditoriumType
        {
            get => _auditoriumType;
            set
            {
                _auditoriumType = value;
                OnPropertyChanged();
                ((RelayCommand)AddAuditoriumCommand).RaiseCanExecuteChanged();
            }
        }

        private string _auditoriumCapacityText;
        public string AuditoriumCapacityText
        {
            get => _auditoriumCapacityText;
            set
            {
                _auditoriumCapacityText = value;
                OnPropertyChanged();
                ((RelayCommand)AddAuditoriumCommand).RaiseCanExecuteChanged();
            }
        }

        private string _auditoriumNote;
        public string AuditoriumNote
        {
            get => _auditoriumNote;
            set
            {
                _auditoriumNote = value;
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand AddAuditoriumCommand { get; private set; }

        public AddAuditoriumViewModel()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            AddAuditoriumCommand = new RelayCommand(ExecuteAddAuditorium, CanExecuteAddAuditorium);
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
            MessageBox.Show("Going back...");
        }

        private bool CanExecuteAddAuditorium(object parameter)
        {
            bool isValidCapacity = int.TryParse(AuditoriumCapacityText, out _);
            return !string.IsNullOrWhiteSpace(AuditoriumName)
                && !string.IsNullOrWhiteSpace(AuditoriumType)
                && isValidCapacity;
        }

        private void ExecuteAddAuditorium(object parameter)
        {
            if (!int.TryParse(AuditoriumCapacityText, out int capacity))
            {
                MessageBox.Show("Invalid capacity value.", "Error");
                return;
            }

            MessageBox.Show($"Adding auditorium:\nName: {AuditoriumName}\nType: {AuditoriumType}\nCapacity: {capacity}\nNote: {AuditoriumNote}", "Auditorium Added");

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
