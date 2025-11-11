using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
            this.DataContext = new ProfileViewModel();
        }
    }
}
