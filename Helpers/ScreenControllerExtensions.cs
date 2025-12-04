using System;
using System.Linq;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Theater_Management_FE.Controllers;

namespace Theater_Management_FE.Helpers
{
    public static class ScreenControllerExtensions
    {
        public static void AutoRegister<TWindow, TController>(this ScreenController sc, IServiceProvider services)
            where TWindow : Window
            where TController : class
        {
            var window = services.GetRequiredService<TWindow>();
            var controller = services.GetRequiredService<TController>();

            // Inject all Set* methods with their corresponding services
            var setMethods = typeof(TController).GetMethods(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                .Where(m => m.Name.StartsWith("Set") && m.GetParameters().Length == 1);

            foreach (var method in setMethods)
            {
                var paramType = method.GetParameters()[0].ParameterType;
                var dependency = services.GetService(paramType);
                if (dependency != null)
                {
                    method.Invoke(controller, new object[] { dependency });
                }
            }

            // bind UI controls
            ControllerBinder.BindControls(window, controller);

            var initMethod = typeof(TWindow).GetMethod("Initialize");
            initMethod?.Invoke(window, new object[] { controller });

            // register
            sc.Register(window, controller);
        }
    }
}
