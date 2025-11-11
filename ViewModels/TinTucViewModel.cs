using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class TinTucViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _newsList;
        public ObservableCollection<string> NewsList
        {
            get => _newsList;
            set { _newsList = value; OnPropertyChanged(); }
        }

        public ICommand NavigateHomeCommand { get; private set; }
        public ICommand NavigateScheduleCommand { get; private set; }
        public ICommand NavigatePromotionsCommand { get; private set; }
        public ICommand NavigatePricesCommand { get; private set; }
        public ICommand NavigateFilmFestivalCommand { get; private set; }
        public ICommand SignUpCommand { get; private set; }
        public ICommand SignInCommand { get; private set; }
        public ICommand ViewNewsDetailCommand { get; private set; }

        public TinTucViewModel()
        {
            NewsList = new ObservableCollection<string>
            {
                "Gian hàng Hội chợ Mùa Thu 2025 - 24/10/2025",
                "CINETOUR 'Tay Anh Giữ Một Vì Sao' - 13/10/2025",
                "Cinetour NCC ngày 9/10/2025 - 10/10/2025"
            };

            NavigateHomeCommand = new RelayCommand(ExecuteNavigation);
            NavigateScheduleCommand = new RelayCommand(ExecuteNavigation);
            NavigatePromotionsCommand = new RelayCommand(ExecuteNavigation);
            NavigatePricesCommand = new RelayCommand(ExecuteNavigation);
            NavigateFilmFestivalCommand = new RelayCommand(ExecuteNavigation);
            SignUpCommand = new RelayCommand(ExecuteSignUp);
            SignInCommand = new RelayCommand(ExecuteSignIn);
            ViewNewsDetailCommand = new RelayCommand(ExecuteViewNewsDetail);
        }

        private void ExecuteNavigation(object parameter)
        {
            MessageBox.Show($"Navigating to: {parameter}", "Navigation");
        }

        private void ExecuteSignUp(object parameter)
        {
            MessageBox.Show("Open Sign Up Window", "Action");
        }

        private void ExecuteSignIn(object parameter)
        {
            MessageBox.Show("Open Sign In Window", "Action");
        }

        private void ExecuteViewNewsDetail(object parameter)
        {
            MessageBox.Show($"Viewing details for news item: {parameter}", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
