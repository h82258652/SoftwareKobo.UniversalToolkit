using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TextBoxEx), new PropertyMetadata(default(CornerRadius)));

        public TextBoxEx()
        {
            DefaultStyleKey = typeof(TextBoxEx);
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
    }
}