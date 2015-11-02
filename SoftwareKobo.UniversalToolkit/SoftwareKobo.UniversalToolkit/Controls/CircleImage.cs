using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class CircleImage : Control
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(ImageSource), typeof(CircleImage), new PropertyMetadata(null, SourceChanged));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(CircleImage), new PropertyMetadata(Stretch.None, StretchChanged));

        private ImageBrush _imageBrush;

        public CircleImage()
        {
            this.DefaultStyleKey = typeof(CircleImage);
        }

        public event ExceptionRoutedEventHandler ImageFailed;

        public event RoutedEventHandler ImageOpened;

        public ImageSource Source
        {
            get
            {
                return (ImageSource)this.GetValue(SourceProperty);
            }
            set
            {
                this.SetValue(SourceProperty, value);
            }
        }

        public Stretch Stretch
        {
            get
            {
                return (Stretch)this.GetValue(StretchProperty);
            }
            set
            {
                this.SetValue(StretchProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._imageBrush = (ImageBrush)this.GetTemplateChild("imageBrush");
            this._imageBrush.ImageOpened += this.ImageOpened;
            this._imageBrush.ImageFailed += this.ImageFailed;
            this._imageBrush.ImageSource = this.Source;
            this._imageBrush.Stretch = this.Stretch;
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircleImage obj = (CircleImage)d;
            ImageSource value = (ImageSource)e.NewValue;

            if (obj._imageBrush != null)
            {
                obj._imageBrush.ImageSource = value;
            }
        }

        private static void StretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CircleImage obj = (CircleImage)d;
            Stretch value = (Stretch)e.NewValue;

            if (obj._imageBrush != null)
            {
                obj._imageBrush.Stretch = value;
            }
        }
    }
}