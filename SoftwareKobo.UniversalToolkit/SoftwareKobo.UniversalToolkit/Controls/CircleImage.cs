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
            DefaultStyleKey = typeof(CircleImage);
        }

        public event ExceptionRoutedEventHandler ImageFailed;

        public event RoutedEventHandler ImageOpened;

        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public Stretch Stretch
        {
            get
            {
                return (Stretch)GetValue(StretchProperty);
            }
            set
            {
                SetValue(StretchProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _imageBrush = (ImageBrush)GetTemplateChild("imageBrush");
            _imageBrush.ImageOpened += ImageOpened;
            _imageBrush.ImageFailed += ImageFailed;
            _imageBrush.ImageSource = Source;
            _imageBrush.Stretch = Stretch;
        }

        private static void SourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (CircleImage)d;
            var value = (ImageSource)e.NewValue;

            if (obj._imageBrush != null)
            {
                obj._imageBrush.ImageSource = value;
            }
        }

        private static void StretchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (CircleImage)d;
            var value = (Stretch)e.NewValue;

            if (obj._imageBrush != null)
            {
                obj._imageBrush.Stretch = value;
            }
        }
    }
}