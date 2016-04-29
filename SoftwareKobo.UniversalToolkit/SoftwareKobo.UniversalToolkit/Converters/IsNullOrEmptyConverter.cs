using System;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    public class IsNullOrEmptyConverter : IValueConverter
    {
        public bool IsInversed
        {
            get;
            set;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var str = value as string;
            return string.IsNullOrEmpty(str) != IsInversed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}