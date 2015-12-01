using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class TextBoxEx : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TextBoxEx), new PropertyMetadata(default(CornerRadius)));

        public TextBoxEx()
        {
            this.DefaultStyleKey = typeof(TextBoxEx);
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)this.GetValue(CornerRadiusProperty);
            }
            set
            {
                this.SetValue(CornerRadiusProperty, value);
            }
        }
    }
}