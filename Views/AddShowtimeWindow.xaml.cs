using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class AddShowtimeWindow : Window
    {
        public AddShowtimeWindow()
        {
            InitializeComponent();
            this.DataContext = new AddShowtimeViewModel();
        }
    }
}
