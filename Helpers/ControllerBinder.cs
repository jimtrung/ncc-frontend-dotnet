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
        }
    }
}
