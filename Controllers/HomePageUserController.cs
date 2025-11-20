using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE.Controllers
{
    public class HomePageUserController
    {
        private ScreenController screenController;
        private AuthService authService;
        private MovieService movieService;
        private AuthTokenUtil authTokenUtil;

        public WrapPanel movieList;
        public Button logoutButton;
        public TextBlock usernameText;

        public void SetScreenController(ScreenController controller)
        {
            screenController = controller;
        }

        public void SetAuthService(AuthService service)
        {
            authService = service;
        }

        public void SetMovieService(MovieService service)
        {
            movieService = service;
        }

        public void SetAuthTokenUtil(AuthTokenUtil util)
        {
            authTokenUtil = util;
        }

        public void HandleOnOpen()
        {
            // Try fetch user silently
            var result = authService.GetUser();
            if (result is not User user || user == null)
            {
                screenController.NavigateTo<Home>();
                return;
            }
            UpdateUserUI(user);

            // Clear movie list first
            movieList.Children.Clear();

            List<Movie> movies = null;
            try
            {
                movies = movieService.GetAllMovies();
            }
            catch
            {
                // fallback: show empty
                movies = new List<Movie>();
            }

            if (movies.Count == 0)
            {
                movieList.Children.Add(new Label { Content = "No movies available 🎬" });
                return;
            }

            foreach (var movie in movies)
            {
                movieList.Children.Add(CreateMovieCard(movie));
            }
        }

        public void HandleSignUpButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<SignUp>();
        }

        public void HandleSignInButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<SignIn>();
        }

        public void HandleLogOutButton(object sender, RoutedEventArgs e)
        {
            authTokenUtil.ClearRefreshToken();
            authTokenUtil.ClearAccessToken();
            UpdateUserUI(null);
            screenController.NavigateTo<Home>();
        }

        private void UpdateUserUI(User user)
        {
            if (usernameText == null || logoutButton == null)
            {
                return;
            }

            if (user == null)
            {
                usernameText.Text = "Khách";
                logoutButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                usernameText.Text = user.Username ?? "Người dùng";
                logoutButton.Visibility = Visibility.Visible;
            }
        }

        public void BindUIControls(WrapPanel movieList, Button logoutButton, TextBlock usernameText)
        {
            this.movieList = movieList;
            this.logoutButton = logoutButton;
            this.usernameText = usernameText;

            this.logoutButton.Click += HandleLogOutButton;
        }

        private StackPanel CreateMovieCard(Movie movie)
        {
            var card = new StackPanel { Margin = new Thickness(10) };

            var poster = new Image
            {
                Width = 204,
                Height = 230,
                Stretch = System.Windows.Media.Stretch.Fill,
                Source = new BitmapImage(new Uri("pack://application:/Resources/Images/cat.jpg"))
            };

            var title = new Label { Content = movie.Name };
            var genres = new Label { Content = string.Join(", ", movie.Genres) };
            var rated = new Label { Content = "Rated: " + movie.Rated + "+" };
            var duration = new Label { Content = movie.Duration + " min" };
            var language = new Label { Content = "Lang: " + movie.Language };

            var desc = new TextBlock
            {
                Text = movie.Description ?? "(No description)",
                Width = 200,
                TextWrapping = TextWrapping.Wrap
            };

            card.Children.Add(poster);
            card.Children.Add(title);
            card.Children.Add(genres);
            card.Children.Add(rated);
            card.Children.Add(duration);
            card.Children.Add(language);
            card.Children.Add(desc);

            return card;
        }
    }
}
