using System.Globalization;
using System.Windows.Data;

namespace Theater_Management_FE.Helpers
{
    public class GuidToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Guid guid ? guid.ToString() : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Guid.TryParse(value as string, out var guid) ? guid : Guid.Empty;
        }
    }
}
