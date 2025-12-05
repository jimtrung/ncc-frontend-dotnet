using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;
using Theater_Management_FE.Controllers;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;
using Theater_Management_FE.Utils;
using Theater_Management_FE.Models;

namespace Theater_Management_FE
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Register global window commands
            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindow));
            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindow, OnCanResizeWindow));
            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindow, OnCanMinimizeWindow));
            CommandManager.RegisterClassCommandBinding(typeof(Window),
                new CommandBinding(SystemCommands.RestoreWindowCommand, OnRestoreWindow, OnCanResizeWindow));

            // Khi app đóng thì đóng tất cả các window
            ShutdownMode = ShutdownMode.OnMainWindowClose;

            var services = new ServiceCollection();

            // HttpClient Factory
            services.AddHttpClient();

            // === Services ===
            services.AddSingleton<AuthTokenUtil>();
            services.AddSingleton<AuthService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                var tokenUtil = sp.GetRequiredService<AuthTokenUtil>();
                return new AuthService(httpClient, tokenUtil);
            });
            services.AddSingleton<MovieService>();
            services.AddSingleton<AuditoriumService>();
            services.AddSingleton<MovieActorService>();
            services.AddSingleton<DirectorService>();
            services.AddSingleton<ActorService>();
            services.AddSingleton<ShowtimeService>();
            services.AddSingleton<TicketService>();
            services.AddSingleton<UserService>();

            // === Controllers ===
            services.AddTransient<SignInController>();
            services.AddTransient<SignUpController>();
            services.AddTransient<HomePageUserController>();
            services.AddTransient<HomePageManagerController>();
            services.AddTransient<HomeController>();
            services.AddTransient<MovieListController>();
            services.AddTransient<AuditoriumListController>();
            services.AddTransient<ProfileController>();
            services.AddTransient<AddMovieController>();
            services.AddTransient<AddAuditoriumController>();
            services.AddTransient<MovieInformationController>();
            services.AddTransient<AuditoriumInformationController>();

            services.AddTransient<ShowtimeListController>();
            services.AddTransient<AddShowtimeController>();
            services.AddTransient<ShowtimeInformationController>();
            services.AddTransient<ShowtimePageController>();
            services.AddTransient<BookTicketController>();
            services.AddTransient<BookedTicketController>();
            services.AddTransient<PayPageController>();

            // === Windows ===
            services.AddTransient<SignIn>();
            services.AddTransient<SignUp>();
            services.AddTransient<HomePageUser>();
            services.AddTransient<HomePageManager>();
            services.AddTransient<Home>();
            services.AddTransient<MovieList>();
            services.AddTransient<AuditoriumList>();
            services.AddTransient<Profile>();
            services.AddTransient<AddMovie>();
            services.AddTransient<AddAuditorium>();
            services.AddTransient<MovieInformation>();
            services.AddTransient<AuditoriumInformation>();

            services.AddTransient<ShowtimeList>();
            services.AddTransient<AddShowtime>();
            services.AddTransient<ShowtimeInformation>();
            services.AddTransient<ShowtimePage>();
            services.AddTransient<BookTicket>();
            services.AddTransient<BookedTicket>();
            services.AddTransient<PayPage>();

            // ScreenController
            services.AddSingleton<ScreenController>();

            Services = services.BuildServiceProvider();

            var sc = Services.GetRequiredService<ScreenController>();

            // --- REGISTER ALL WINDOWS + CONTROLLERS ---
            sc.AutoRegister<Home, HomeController>(Services);
            sc.AutoRegister<SignIn, SignInController>(Services);
            sc.AutoRegister<SignUp, SignUpController>(Services);
            sc.AutoRegister<HomePageUser, HomePageUserController>(Services);
            sc.AutoRegister<HomePageManager, HomePageManagerController>(Services);
            sc.AutoRegister<MovieList, MovieListController>(Services);
            sc.AutoRegister<AuditoriumList, AuditoriumListController>(Services);
            sc.AutoRegister<Profile, ProfileController>(Services);
            sc.AutoRegister<AddMovie, AddMovieController>(Services);
            sc.AutoRegister<AddAuditorium, AddAuditoriumController>(Services);
            sc.AutoRegister<MovieInformation, MovieInformationController>(Services);
            sc.AutoRegister<AuditoriumInformation, AuditoriumInformationController>(Services);

            sc.AutoRegister<ShowtimeList, ShowtimeListController>(Services);
            sc.AutoRegister<AddShowtime, AddShowtimeController>(Services);
            sc.AutoRegister<ShowtimeInformation, ShowtimeInformationController>(Services);
            sc.AutoRegister<ShowtimePage, ShowtimePageController>(Services);
            sc.AutoRegister<BookTicket, BookTicketController>(Services);
            sc.AutoRegister<BookedTicket, BookedTicketController>(Services);
            sc.AutoRegister<PayPage, PayPageController>(Services);

            // --- STARTUP MAIN WINDOW ---
            sc.NavigateTo<Home>();
        }

        private void OnCloseWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow((Window)target);
        }

        private void OnCanResizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((Window)sender).ResizeMode == ResizeMode.CanResize || ((Window)sender).ResizeMode == ResizeMode.CanResizeWithGrip;
        }

        private void OnCanMinimizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((Window)sender).ResizeMode != ResizeMode.NoResize;
        }

        private void OnMaximizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow((Window)target);
        }

        private void OnMinimizeWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow((Window)target);
        }

        private void OnRestoreWindow(object target, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow((Window)target);
        }
    }
}
