using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Theater_Management_FE.Helpers;

namespace Theater_Management_FE.Controllers
{
    public class ScreenController
    {
        private readonly Stack<Window> _history = new();
        private readonly Dictionary<Type, Window> _windows = new();
        private readonly Dictionary<Window, object> _controllers = new();

        public void Register<TWindow, TController>(TWindow window, TController controller)
            where TWindow : Window
        {
            _windows[typeof(TWindow)] = window;
            _controllers[window] = controller!;
        }

        public void NavigateTo<TWindow>(bool closeCurrent = true) where TWindow : Window
        {
            if (!_windows.TryGetValue(typeof(TWindow), out var newWindow))
                throw new Exception($"Window {typeof(TWindow).Name} not registered");

            NavigateTo(newWindow, closeCurrent);
        }

        public void NavigateTo(Window newWindow, bool closeCurrent = true)
        {
            var current = Application.Current.MainWindow ??
                          (Application.Current.Windows.Count > 0 ? Application.Current.Windows[Application.Current.Windows.Count - 1] : null);

            newWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            newWindow.Show();
            Application.Current.MainWindow = newWindow;

            if (closeCurrent && current != null && current != newWindow)
            {
                _history.Push(current);
                current.Hide();
            }

            if (_controllers.TryGetValue(newWindow, out var controller))
            {
                var method = controller.GetType().GetMethod("HandleOnOpen");
                method?.Invoke(controller, null);
            }
        }

        public TController? GetController<TController>() where TController : class
        {
            foreach (var controller in _controllers.Values)
            {
                if (controller is TController typedController)
                {
                    return typedController;
                }
            }
            return null;
        }

        public void GoBack()
        {
            if (_history.Count == 0) return;

            var previous = _history.Pop();
            previous.Show();
            Application.Current.MainWindow = previous;

            if (_controllers.TryGetValue(previous, out var controller))
            {
                var method = controller.GetType().GetMethod("HandleOnOpen");
                method?.Invoke(controller, null);
            }
        }
    }
}
