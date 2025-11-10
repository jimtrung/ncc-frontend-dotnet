using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace Theater_Management_FE.Services;

public class NavigationService
{
    private readonly IServiceProvider _provider;
    private readonly Dictionary<Type, Window> _openWindows = new();

    public NavigationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void Show<T>() where T : Window
    {
        if (_openWindows.TryGetValue(typeof(T), out var existing))
        {
            existing.Activate();
            return;
        }

        var window = _provider.GetRequiredService<T>();
        window.Closed += (s, e) => _openWindows.Remove(typeof(T));
        _openWindows[typeof(T)] = window;
        window.Show();
    }

    public void ShowDialog<T>() where T : Window
    {
        var window = _provider.GetRequiredService<T>();
        window.ShowDialog();
    }

    public void Close<T>() where T : Window
    {
        if (_openWindows.TryGetValue(typeof(T), out var win))
        {
            win.Close();
        }
    }
}
