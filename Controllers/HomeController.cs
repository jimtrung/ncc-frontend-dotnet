using System.Windows;
using System.Windows.Controls;
using Theater_Management_FE.Services;
using Theater_Management_FE.Models;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class HomeController
    {
        private ScreenController _screenController;
        private AuthService _authService;
        private AuthTokenUtil _authTokenUtil;

        public void SetScreenController(ScreenController screenController)
        {
            _screenController = screenController;
        }

        public void SetAuthService(AuthService authService)
        {
            _authService = authService;
        }

        public void SetAuthTokenUtil(AuthTokenUtil authTokenUtil)
        {
            _authTokenUtil = authTokenUtil;
        }

        public TextBlock titleLabel;
        public Button signupButton;
        public Button signinButton;
        public Button settingsButton;

        public void HandleOnOpen()
        {
            // Only try to fetch user if token exists
            var token = _authTokenUtil.LoadAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            User user = null;

            try
            {
                user = _authService.GetUser() as User;
            }
            catch (Exception) { }

            if (user != null)
            {
                if (user.Role == UserRole.user)
                    _screenController.NavigateTo<HomePageUser>();

                if (user.Role == UserRole.administrator)
                    _screenController.NavigateTo<HomePageManager>();
            }
        }

        public void HandleSignUpButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<SignUp>();
        }

        public void HandleSignInButton(object sender, RoutedEventArgs e)
        {
            _screenController.NavigateTo<SignIn>();
        }

        public void BindUIControls(TextBlock titleLabel, Button signupButton, Button signinButton) 
        {
            this.titleLabel = titleLabel;
            this.signupButton = signupButton;
            this.signinButton = signinButton;

            this.signupButton.Click += HandleSignUpButton;
            this.signinButton.Click += HandleSignInButton;
        }
    }
}
