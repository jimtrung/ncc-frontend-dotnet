using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class MovieListWindow : Window
    {
        public MovieListWindow()
        {
            InitializeComponent();
            this.DataContext = new MovieListViewModel();
        }
    }
}
