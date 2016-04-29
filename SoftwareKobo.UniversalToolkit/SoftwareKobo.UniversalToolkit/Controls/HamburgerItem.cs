using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    [ContentProperty(Name = nameof(Text))]
    public sealed class HamburgerItem : Button
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(HamburgerItem), new PropertyMetadata(null));

        public static readonly DependencyProperty PointerOverBackgroundProperty = DependencyProperty.Register(nameof(PointerOverBackground), typeof(Brush), typeof(HamburgerItem), new PropertyMetadata(null));

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(HamburgerItem), new PropertyMetadata(null));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(HamburgerItem), new PropertyMetadata(null));

        public HamburgerItem()
        {
            DefaultStyleKey = typeof(HamburgerItem);
        }

        public IconElement Icon
        {
            get
            {
                return (IconElement)GetValue(IconProperty);
            }
            set
            {
                SetValue(IconProperty, value);
            }
        }

        public Brush PointerOverBackground
        {
            get
            {
                return (Brush)GetValue(PointerOverBackgroundProperty);
            }
            set
            {
                SetValue(PointerOverBackgroundProperty, value);
            }
        }

        public Brush PressedBackground
        {
            get
            {
                return (Brush)GetValue(PressedBackgroundProperty);
            }
            set
            {
                SetValue(PressedBackgroundProperty, value);
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }
    }
}