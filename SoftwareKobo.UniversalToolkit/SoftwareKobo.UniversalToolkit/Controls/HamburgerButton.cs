using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class HamburgerButton : ToggleButton
    {
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(Thickness), typeof(HamburgerButton), new PropertyMetadata(new Thickness()));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(HamburgerButton), new PropertyMetadata(default(double)));

        public HamburgerButton()
        {
            this.DefaultStyleKey = typeof(HamburgerButton);
            this.Checked += this.HamburgerButton_Checked;
            this.Unchecked += this.HamburgerButton_Unchecked;
        }

        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)this.GetValue(ContentMarginProperty);
            }
            set
            {
                this.SetValue(ContentMarginProperty, value);
            }
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