using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class HamburgerMenu : ToggleButton
    {
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(default(double)));

        public HamburgerMenu()
        {
            this.DefaultStyleKey = typeof(HamburgerMenu);
            this.Checked += this.HamburgerMenu_Checked;
            this.Unchecked += this.HamburgerMenu_Unchecked;
        }
        
        public double IconSize
        {
            get
            {
                return (double)this.GetValue(IconSizeProperty);
            }
            set
            {
                this.SetValue(IconSizeProperty, value);
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