using System.Globalization;
using System.Windows.Data;

namespace Theater_Management_FE.Helpers
{
    public class DateToStringConverter : IValueConverter
    {
        private readonly string _format;
        public DateToStringConverter() => _format = "dd/MM/yyyy";
        public DateToStringConverter(string format) => _format = format;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is DateTime dt ? dt.ToString(_format) : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (DateTime.TryParseExact(value as string, _format, culture, DateTimeStyles.None, out var dt))
                return dt;
            return DateTime.MinValue;
        }
    }
}
