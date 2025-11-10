using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using Theater_Management_FE.Services;
//using Theater_Management_FE.ViewModels;
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
        //services.AddTransient<SignInViewModel>();
        //services.AddTransient<SignUpViewModel>();
        //services.AddTransient<HomePageUserViewModel>();
        //services.AddTransient<HomePageManagerViewModel>();
        //services.AddTransient<MovieListViewModel>();
        //services.AddTransient<AuditoriumListViewModel>();
        //services.AddTransient<ProfileViewModel>();
        //services.AddTransient<ShowtimeListViewModel>();
        //services.AddTransient<TinTucViewModel>();
        //services.AddTransient<AddMovieViewModel>();
        //services.AddTransient<AddAuditoriumViewModel>();
        //services.AddTransient<MovieInformationViewModel>();
        //services.AddTransient<AuditoriumInformationViewModel>();

        // === Windows (Transient – mỗi lần mở là mới) ===
        services.AddTransient<SignInWindow>();
        services.AddTransient<SignUpWindow>();
        services.AddTransient<HomePageUserWindow>();
        services.AddTransient<HomePageManagerWindow>();
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

        var nav = Services.GetRequiredService<NavigationService>();
        var tokenUtil = Services.GetRequiredService<AuthTokenUtil>();
        nav.Show<HomeWindow>();
    }
}
