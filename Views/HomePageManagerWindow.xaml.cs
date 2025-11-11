using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class HomePageManagerWindow : Window
    {
        public HomePageManagerWindow()
        {
            InitializeComponent();
            this.DataContext = new HomePageManagerViewModel();
        }
    }
}
