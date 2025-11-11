using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class AddAuditoriumWindow : Window
    {
        public AddAuditoriumWindow()
        {
            InitializeComponent();
            this.DataContext = new AddAuditoriumViewModel();
        }
    }
}
