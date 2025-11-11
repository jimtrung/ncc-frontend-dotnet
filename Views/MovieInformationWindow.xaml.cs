using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class MovieInformationWindow : Window
    {
        public MovieInformationWindow()
        {
            InitializeComponent();
            this.DataContext = new MovieInformationViewModel();
        }
    }
}
