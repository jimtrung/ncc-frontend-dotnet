using System.Windows;
using Theater_Management_FE.ViewModels;

namespace Theater_Management_FE.Views
{
    public partial class SignInWindow : Window
    {
        public SignInWindow()
        {
            InitializeComponent();
            this.DataContext = new SignInViewModel();
        }
    }
}
