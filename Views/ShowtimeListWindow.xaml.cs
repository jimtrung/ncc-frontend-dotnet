using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class ShowtimeListWindow : Window
    {
        public ShowtimeListWindow()
        {
            InitializeComponent();
            this.DataContext = new ShowtimeListViewModel();
        }
    }
}
