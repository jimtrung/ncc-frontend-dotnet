using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Documents;
using Theater_Management_FE.Models;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;

namespace Theater_Management_FE.Controllers
{
    public class HomePageUserController
    {
        private ScreenController screenController;
        private AuthService authService;
        private MovieService movieService;
        private AuthTokenUtil authTokenUtil;

        public WrapPanel movieList;
        public Button profileButton;
        public Button logoutButton;
        public TextBlock usernameText;

        private bool _isInitialized = false;

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

        public async void HandleOnOpen()
        {
            try
            {
                if (!_isInitialized)
                {
                    if (profileButton != null) profileButton.Click += HandleProfileButton;
                    if (logoutButton != null) logoutButton.Click += HandleLogOutButton;
                    _isInitialized = true;
                }

                // Check if required services are available
                if (authService == null || movieService == null || screenController == null)
                {
                    System.Diagnostics.Debug.WriteLine("Required services not initialized");
                    return;
                }

                // Try fetch user silently
                User user = null;
                try
                {
                    var result = authService.GetUser();
                    user = result as User;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to get user: {ex.Message}");
                    // Treat as guest or navigate to Home if strict
                    screenController.NavigateTo<Home>();
                    return;
                }

                if (user == null)
                {
                    screenController.NavigateTo<Home>();
                    return;
                }
                
                UpdateUserUI(user);

                // Clear movie list and show loading
                if (movieList != null) 
                {
                    movieList.Children.Clear();
                    movieList.Children.Add(new TextBlock 
                    { 
                        Text = "Loading movies...", 
                        Foreground = Brushes.White, 
                        FontSize = 16,
                        Margin = new Thickness(10)
                    });
                }

                List<Movie> movies = null;
                string errorMessage = null;
                try
                {
                    movies = await movieService.GetAllMoviesAsync();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load movies: {ex.Message}");
                    errorMessage = "Unable to load movies. Please try again later.";
                    movies = new List<Movie>();
                }

                // Clear loading message
                if (movieList != null) movieList.Children.Clear();

                if (movies == null || movies.Count == 0)
                {
                    if (movieList != null)
                    {
                        var msg = errorMessage ?? "No movies available 🎬";
                        var color = errorMessage != null ? Brushes.Red : Brushes.White;
                        movieList.Children.Add(new TextBlock 
                        { 
                            Text = msg, 
                            Foreground = color, 
                            FontSize = 16,
                            Margin = new Thickness(10)
                        });
                    }
                    return;
                }

                foreach (var movie in movies)
                {
                    if (movieList != null) movieList.Children.Add(CreateMovieCard(movie));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An error occurred in HomePageUser: {ex.Message}");
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

        public void HandleProfileButton(object sender, RoutedEventArgs e)
        {
            screenController.NavigateTo<Profile>();
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
            if (usernameText == null)
            {
                return;
            }

            if (user == null)
            {
                usernameText.Text = "Khách";
                if (logoutButton != null) logoutButton.Visibility = Visibility.Collapsed;
                if (profileButton != null) profileButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                usernameText.Text = user.Username ?? "Người dùng";
                if (logoutButton != null) logoutButton.Visibility = Visibility.Visible;
                if (profileButton != null) profileButton.Visibility = Visibility.Visible;
            }
        }

        // Hàm tạo thẻ phim ở trang chủ
        private StackPanel CreateMovieCard(Movie movie)
        {
            var card = new StackPanel 
            { 
                Width = 204,
                Margin = new Thickness(12),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            // Tạo poster
            var posterBorder = new System.Windows.Controls.Border
            {
                Width = 204,
                Height = 280,
                CornerRadius = new CornerRadius(8),
                ClipToBounds = true,
                Background = System.Windows.Media.Brushes.LightGray
            };

            var imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Images", $"{movie.Id}.jpg");
            var imageUri = System.IO.File.Exists(imagePath) 
                ? new Uri(imagePath) 
                : new Uri("pack://application:,,,/Resources/Images/cat.jpg");

            var poster = new Image
            {
                Width = 204,
                Height = 280,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
                Source = new BitmapImage(imageUri)
            };
            posterBorder.Child = poster;

            var title = new TextBlock
            {
                Text = movie.Name,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 10, 0, 4),
                TextWrapping = TextWrapping.Wrap,
                MaxHeight = 40,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Foreground = Brushes.White
            };

            var genresText = new TextBlock
            {
                Text = movie.VietnameseGenres,
                FontSize = 12,
                Foreground = Brushes.LightGray,
                TextTrimming = TextTrimming.CharacterEllipsis,
                Margin = new Thickness(0, 0, 0, 4)
            };

            var infoText = new TextBlock
            {
                FontSize = 12,
                Foreground = Brushes.LightGray,
                Margin = new Thickness(0, 0, 0, 4)
            };
            infoText.Inlines.Add(new Run { Text = $"Giới hạn độ tuổi: {movie.Rated}+ • " });
            infoText.Inlines.Add(new Run { Text = $"{movie.Duration} phút" });

            var languageText = new TextBlock
            {
                Text = $"Ngôn ngữ: {movie.Language}",
                FontSize = 12,
                Foreground = Brushes.LightGray
            };

            card.Children.Add(posterBorder);
            card.Children.Add(title);
            card.Children.Add(genresText);
            card.Children.Add(infoText);
            card.Children.Add(languageText);

            return card;
        }

        public void BindUIControls(WrapPanel movieList, Button profileButton, Button logoutButton, TextBlock usernameText)
        {
            this.movieList = movieList;
            this.profileButton = profileButton;
            this.logoutButton = logoutButton;
            this.usernameText = usernameText;

            if (this.profileButton != null)
            {
                this.profileButton.Click -= HandleProfileButton;
                this.profileButton.Click += HandleProfileButton;
            }

            if (this.logoutButton != null)
            {
                this.logoutButton.Click -= HandleLogOutButton;
                this.logoutButton.Click += HandleLogOutButton;
            }
        }
    }
}
