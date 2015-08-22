using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Triggers
{
    public class OrientationStateTrigger : StateTriggerBase
    {
        public OrientationStateTrigger()
        {
            DisplayInformation.GetForCurrentView().OrientationChanged += OrientationStateTrigger_OrientationChanged;
        }

        private void OrientationStateTrigger_OrientationChanged(DisplayInformation sender, object args)
        {
            this.SetActive(sender.CurrentOrientation == Orientation);
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(DisplayOrientations), typeof(OrientationStateTrigger), new PropertyMetadata(DisplayOrientations.None, OrientationChanged));

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (OrientationStateTrigger)d;
            var value = (DisplayOrientations)e.NewValue;
            obj.SetActive(value == obj.Orientation);
        }

        public DisplayOrientations Orientation
        {
            get
            {
                return (DisplayOrientations)this.GetValue(OrientationProperty);
            }
            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }
    }
}