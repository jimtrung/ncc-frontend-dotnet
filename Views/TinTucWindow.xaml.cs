using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class TinTucWindow : Window
    {
        public TinTucWindow()
        {
            InitializeComponent();
            this.DataContext = new TinTucViewModel();
        }
    }
}
