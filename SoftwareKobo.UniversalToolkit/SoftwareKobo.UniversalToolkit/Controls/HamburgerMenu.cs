using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class HamburgerMenu : ToggleButton
    {
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public HamburgerMenu()
        {
            DefaultStyleKey = typeof(HamburgerMenu);
            Checked += HamburgerMenu_Checked;
            Unchecked += HamburgerMenu_Unchecked;
        }

        public double IconSize
        {
            get
            {
                return (double)GetValue(IconSizeProperty);
            }
            set
            {
                SetValue(IconSizeProperty, value);
            }
        }

        private void HamburgerMenu_Checked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Checked", true);
        }

        private void HamburgerMenu_Unchecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Unchecked", true);
        }
    }
}