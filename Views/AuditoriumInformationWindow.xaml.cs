using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class AuditoriumInformationWindow : Window
    {
        public AuditoriumInformationWindow()
        {
            InitializeComponent();
            this.DataContext = new AuditoriumInformationViewModel();
        }
    }
}
