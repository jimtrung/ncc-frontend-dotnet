using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using Theater_Management_FE.DTOs;
using Theater_Management_FE.Helpers;

namespace Theater_Management_FE.Controllers
{
    public class EventListController
    {
        private ScreenController _screenController;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void BindUIControls()
        {
        }
    }
}
