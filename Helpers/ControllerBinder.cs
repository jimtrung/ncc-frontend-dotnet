// Vì trong WPF không có tính năng tự bind như JavaFx @FXML nên phải bind thủ công
using System.Reflection;
using System.Windows;

namespace Theater_Management_FE.Helpers
{
    public static class ControllerBinder
    {
        public static void BindControls(Window window, object controller)
        {
            var type = controller.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var control = window.FindName(field.Name);
                if (control != null && field.FieldType.IsAssignableFrom(control.GetType()))
                {
                    field.SetValue(controller, control);
                }
            }

            // If controller exposes BindUIControls, try to resolve parameters by name and invoke it
            var bindMethod = type.GetMethod("BindUIControls", BindingFlags.Public | BindingFlags.Instance);
            if (bindMethod != null)
            {
                try
                {
                    var parameters = bindMethod.GetParameters();
                    var args = new object[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];
                        // Prefer control looked up by parameter name
                        var control = window.FindName(p.Name);

                        // Fallback to already-bound field with same name
                        if (control == null)
                        {
                            var field = fields.FirstOrDefault(f => f.Name == p.Name);
                            control = field?.GetValue(controller);
                        }

                        if (control == null || !p.ParameterType.IsAssignableFrom(control.GetType()))
                        {
                            // Cannot resolve this parameter, abort binding to avoid runtime errors
                            return;
                        }

                        args[i] = control;
                    }

                    bindMethod.Invoke(controller, args);
                }
                catch (Exception)
                {
                    // Swallow binding errors to avoid breaking window creation
                }
            }
        }
    }
}
