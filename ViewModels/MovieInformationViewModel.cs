using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class MovieInformationViewModel : INotifyPropertyChanged
    {
        private string _movieName;
        public string MovieName
        {
            get => _movieName;
            set { _movieName = value; OnPropertyChanged(); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(); }
        }

        private string _directorId;
        public string DirectorId
        {
            get => _directorId;
            set { _directorId = value; OnPropertyChanged(); }
        }

        private string _genres;
        public string Genres
        {
            get => _genres;
            set { _genres = value; OnPropertyChanged(); }
        }

        private string _premiere;
        public string Premiere
        {
            get => _premiere;
            set { _premiere = value; OnPropertyChanged(); }
        }

        private string _duration;
        public string Duration
        {
            get => _duration;
            set { _duration = value; OnPropertyChanged(); }
        }

        private string _ratedAge;
        public string RatedAge
        {
            get => _ratedAge;
            set { _ratedAge = value; OnPropertyChanged(); }
        }

        private string _language;
        public string Language
        {
            get => _language;
            set { _language = value; OnPropertyChanged(); }
        }

        public ICommand BackCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public MovieInformationViewModel()
        {
            BackCommand = new RelayCommand(ExecuteBack);
            EditCommand = new RelayCommand(ExecuteEdit);
            DeleteCommand = new RelayCommand(ExecuteDelete);

            MovieName = "The Gemini Project";
            Description = "A thrilling sci-fi adventure about an AI awakening and its quest for self-discovery in a futuristic theater system.";
            DirectorId = "DIR-456";
            Genres = "Sci-Fi, Action, Drama";
            Premiere = "2025-12-01 19:00:00";
            Duration = "150";
            RatedAge = "16+";
            Language = "English";
        }

        private void ExecuteBack(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        private void ExecuteEdit(object parameter)
        {
            MessageBox.Show($"Saving changes to: {MovieName}", "Action: Edit");
        }

        private void ExecuteDelete(object parameter)
        {
            MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{MovieName}'?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Movie deleted.", "Action: Delete");
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
