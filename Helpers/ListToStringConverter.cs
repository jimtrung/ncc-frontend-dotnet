using System.Globalization;
using System.Windows.Data;

namespace Theater_Management_FE.Helpers
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> list) return string.Join(", ", list);
            if (value is IEnumerable<string> enumerable) return string.Join(", ", enumerable);
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as string;
            if (string.IsNullOrWhiteSpace(str)) return new List<string>();
            return new List<string>(str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
    }
}
