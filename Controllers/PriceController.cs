using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Models;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class PriceController
    {
        private Button _priceButton;
        public Button homeButton;
        private ScreenController screenController;

        public void Bind(Button priceButton)
        {
            _priceButton = priceButton;
            _priceButton.Click += PriceButton_Click;
        }

        private void PriceButton_Click(object sender, RoutedEventArgs e)
        {
            Price priceWindow = new Price();
            priceWindow.Show();
        }
        public void SetScreenController(ScreenController controller)
        {
            screenController = controller;
        }
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<HomePageUser>();
        }
        public void BindUIControls(Button homeButton)
        {
            this.homeButton = homeButton;
            if (this.homeButton != null)
            {
                this.homeButton.Click += homeButton_Click;
            }
        }
    }
}
