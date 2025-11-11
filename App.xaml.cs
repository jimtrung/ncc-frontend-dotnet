using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Theater_Management_FE.Services;
using Theater_Management_FE.ViewModels;
using Theater_Management_FE.Views;

namespace Theater_Management_FE;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();

        // === Services (Singleton) ===
        services.AddSingleton<AuthTokenUtil>();
        services.AddSingleton<AuthService>();
        services.AddSingleton<MovieService>();
        services.AddSingleton<AuditoriumService>();
        services.AddSingleton<MovieActorService>();
        services.AddSingleton<DirectorService>();
        services.AddSingleton<ActorService>();

        // === ViewModels (Transient) ===
        services.AddTransient<SignInViewModel>();
        services.AddTransient<SignUpViewModel>();
        services.AddTransient<HomePageUserViewModel>();
        services.AddTransient<HomePageManagerViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<MovieListViewModel>();
        services.AddTransient<AuditoriumListViewModel>();
        services.AddTransient<ProfileViewModel>();
        services.AddTransient<ShowtimeListViewModel>();
        services.AddTransient<TinTucViewModel>();
        services.AddTransient<AddMovieViewModel>();
        services.AddTransient<AddAuditoriumViewModel>();
        services.AddTransient<MovieInformationViewModel>();
        services.AddTransient<AuditoriumInformationViewModel>();

        // === Windows (Transient – mỗi lần mở là mới) ===
        services.AddTransient<SignInWindow>();
        services.AddTransient<SignUpWindow>();
        services.AddTransient<HomePageUserWindow>();
        services.AddTransient<HomePageManagerWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<MovieListWindow>();
        services.AddTransient<AuditoriumListWindow>();
        services.AddTransient<ProfileWindow>();
        services.AddTransient<ShowtimeListWindow>();
        services.AddTransient<TinTucWindow>();
        services.AddTransient<AddMovieWindow>();
        services.AddTransient<AddAuditoriumWindow>();
        services.AddTransient<MovieInformationWindow>();
        services.AddTransient<AuditoriumInformationWindow>();

        // === Navigation Service (Singleton) ===
        services.AddSingleton<NavigationService>();

        Services = services.BuildServiceProvider();

        // === Try auto-refresh token ===
        //_ = Task.Run(async () =>
        //{
        //    try
        //    {
        //        var auth = Services.GetRequiredService<AuthService>();
        //        var util = Services.GetRequiredService<AuthTokenUtil>();
        //        var token = await auth.RefreshAsync();
        //        util.SaveAccessToken(token);
        //    }
        //    catch { /* ignore */ }
        //});

        // --- STARTUP FIX: Use DI to create and set the MainWindow ---

        // 1. Get the initial window instance
        var initialWindow = Services.GetRequiredService<MainWindow>();

        // 2. Set it as the application's main window (CRUCIAL for keeping the app running)
        this.MainWindow = initialWindow;

        // 3. Show the window
        // Use the NavigationService only if it correctly sets the DataContext and shows the window.
        // If not, you may need to set DataContext here and use initialWindow.Show().
        var nav = Services.GetRequiredService<NavigationService>();
        nav.Show<MainWindow>();

        // The above nav.Show<HomeWindow>() is kept, assuming your NavigationService handles
        // setting the DataContext and showing the window correctly after we set MainWindow.
        // If the problem persists, replace the nav.Show line with:
        // initialWindow.DataContext = Services.GetRequiredService<HomeViewModel>();
        // initialWindow.Show();

        // --- END OF STARTUP FIX ---
    }
}
