using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Converters
{
    public class ItemClickEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var args = value as ItemClickEventArgs;
            return args == null ? null : args.ClickedItem;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}