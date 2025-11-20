using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Windows;
using Theater_Management_FE.Controllers;
using Theater_Management_FE.Helpers;
using Theater_Management_FE.Services;
using Theater_Management_FE.Views;

namespace Theater_Management_FE
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

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

            // --- STARTUP MAIN WINDOW ---
            // Use ScreenController so that navigation & HandleOnOpen are consistent
            sc.NavigateTo<Home>();
        }
    }
}
