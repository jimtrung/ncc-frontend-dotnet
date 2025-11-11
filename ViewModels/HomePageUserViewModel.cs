using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.ViewModels
{
    public class HomePageUserViewModel : INotifyPropertyChanged
    {
        public ICommand HomeCommand { get; private set; }
        public ICommand ShowTimesCommand { get; private set; }
        public ICommand NewsCommand { get; private set; }
        public ICommand PromotionCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand ProfileImageCommand { get; private set; }
        public ICommand SeeAllNowShowingCommand { get; private set; }
        public ICommand SeeAllPromotionCommand { get; private set; }
        public ICommand SeeAllEventsCommand { get; private set; }

        public HomePageUserViewModel()
        {
            HomeCommand = new RelayCommand(ExecuteHome);
            ShowTimesCommand = new RelayCommand(ExecuteShowTimes);
            NewsCommand = new RelayCommand(ExecuteNews);
            PromotionCommand = new RelayCommand(ExecutePromotion);
            AboutCommand = new RelayCommand(ExecuteAbout);
            ProfileImageCommand = new RelayCommand(ExecuteProfileImage);
            SeeAllNowShowingCommand = new RelayCommand(ExecuteSeeAllNowShowing);
            SeeAllPromotionCommand = new RelayCommand(ExecuteSeeAllPromotion);
            SeeAllEventsCommand = new RelayCommand(ExecuteSeeAllEvents);
        }

        private void ExecuteHome(object parameter)
        {
            MessageBox.Show("Navigate to Home.", "Navigation");
        }

        private void ExecuteShowTimes(object parameter)
        {
            MessageBox.Show("Navigate to Show Times page.", "Navigation");
        }

        private void ExecuteNews(object parameter)
        {
            MessageBox.Show("Navigate to News page.", "Navigation");
        }

        private void ExecutePromotion(object parameter)
        {
            MessageBox.Show("Navigate to Promotions page.", "Navigation");
        }

        private void ExecuteAbout(object parameter)
        {
            MessageBox.Show("Navigate to About page.", "Navigation");
        }

        private void ExecuteProfileImage(object parameter)
        {
            MessageBox.Show("Open User Profile or Login/Logout menu.", "Action");
        }

        private void ExecuteSeeAllNowShowing(object parameter)
        {
            MessageBox.Show("Navigate to full Now Showing list.", "Action");
        }

        private void ExecuteSeeAllPromotion(object parameter)
        {
            MessageBox.Show("Navigate to full Promotion list.", "Action");
        }

        private void ExecuteSeeAllEvents(object parameter)
        {
            MessageBox.Show("Navigate to full Events list.", "Action");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
