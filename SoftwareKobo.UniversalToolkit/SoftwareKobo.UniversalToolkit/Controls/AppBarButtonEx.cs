using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    [ContentProperty(Name = nameof(IconContent))]
    public sealed class AppBarButtonEx : AppBarButton
    {
        public static readonly DependencyProperty IconContentProperty = DependencyProperty.Register(nameof(IconContent), typeof(object), typeof(AppBarButtonEx), new PropertyMetadata(null));

        public AppBarButtonEx()
        {
            DefaultStyleKey = typeof(AppBarButtonEx);
        }

        public object IconContent
        {
            get
            {
                return GetValue(IconContentProperty);
            }
            set
            {
                SetValue(IconContentProperty, value);
            }
        }
    }
}