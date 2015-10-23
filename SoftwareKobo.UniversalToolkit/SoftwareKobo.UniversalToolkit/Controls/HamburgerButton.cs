using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class HamburgerButton : ToggleButton
    {
        public HamburgerButton()
        {
            this.DefaultStyleKey = typeof(HamburgerButton);
            this.Checked += this.HamburgerButton_Checked;
            this.Unchecked += this.HamburgerButton_Unchecked;
        }

        private void HamburgerButton_Checked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Checked", true);
        }

        private void HamburgerButton_Unchecked(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Unchecked", true);
        }
    }
}