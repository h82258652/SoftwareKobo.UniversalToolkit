using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Triggers
{
    /// <summary>
    /// 设备方向触发器。
    /// </summary>
    public class OrientationStateTrigger : StateTriggerBase
    {
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(OrientationStateTrigger), new PropertyMetadata(Orientation.None, OrientationChanged));

        public OrientationStateTrigger()
        {
            if (DesignMode.DesignModeEnabled == false)
            {
                DisplayInformation.GetForCurrentView().OrientationChanged += OrientationStateTrigger_OrientationChanged;
            }
        }

        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.GetValue(OrientationProperty);
            }
            set
            {
                this.SetValue(OrientationProperty, value);
            }
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (OrientationStateTrigger)d;
            obj.UpdateState();
        }

        private void OrientationStateTrigger_OrientationChanged(DisplayInformation sender, object args)
        {
            UpdateState();
        }

        private void UpdateState()
        {
            if (DesignMode.DesignModeEnabled == false)
            {
                var currentOrientation = DisplayInformation.GetForCurrentView().CurrentOrientation;
                switch (currentOrientation)
                {
                    case DisplayOrientations.Landscape:
                    case DisplayOrientations.LandscapeFlipped:
                        this.SetActive(this.Orientation == Orientation.Landscape);
                        break;

                    case DisplayOrientations.Portrait:
                    case DisplayOrientations.PortraitFlipped:
                        this.SetActive(this.Orientation == Orientation.Portrait);
                        break;

                    default:
                        this.SetActive(this.Orientation == Orientation.None);
                        break;
                }
            }
        }
    }
}