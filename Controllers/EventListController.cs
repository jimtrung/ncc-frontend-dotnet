using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Theater_Management_FE.DTOs;
using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Views;
using Theater_Management_FE.Services;

namespace Theater_Management_FE.Controllers
{
    public class EventListController
    {
        private ScreenController _screenController;
        private bool _isInitialized = false;

        // UI Elements - Public for ControllerBinder
        public Button btnBackHome;
        public Button btnShowTime;
        public Button btnNews;
        public Button btnPromotion;
        public Button btnPrice;
        public Button btnCinema;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void HandleOnOpen()
        {
            if (!_isInitialized)
            {
                if (btnBackHome != null) btnBackHome.Click += (s, e) => _screenController.NavigateTo<HomePageUser>();
                if (btnShowTime != null) btnShowTime.Click += (s, e) => _screenController.NavigateTo<ShowtimePage>();
                if (btnNews != null) btnNews.Click += (s, e) => _screenController.NavigateTo<TinTuc>();
                // btnPromotion is current page
                if (btnCinema != null) btnCinema.Click += (s, e) => _screenController.NavigateTo<HomePageUser>(); 

                _isInitialized = true;
            }
        }

        public void BindUIControls()
        {
            // method kept if needed but logic is in HandleOnOpen
        }
    }
}
