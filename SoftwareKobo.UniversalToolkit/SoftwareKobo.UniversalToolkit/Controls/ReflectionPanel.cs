using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    [ContentProperty(Name = nameof(Content))]
    public sealed class ReflectionPanel : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(UIElement), typeof(ReflectionPanel), new PropertyMetadata(null));

        public static readonly DependencyProperty ReflectionSpacingProperty = DependencyProperty.Register(nameof(ReflectionSpacing), typeof(double), typeof(ReflectionPanel), new PropertyMetadata(default(double), ReflectionSpacingChanged));

        private ContentControl _contentBorder;

        private Image _reflectionImage;

        private TranslateTransform _spacingTransform;

        public ReflectionPanel()
        {
            DefaultStyleKey = typeof(ReflectionPanel);
        }

        public UIElement Content
        {
            get
            {
                return (UIElement)GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        public double ReflectionSpacing
        {
            get
            {
                return (double)GetValue(ReflectionSpacingProperty);
            }
            set
            {
                SetValue(ReflectionSpacingProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            var rootLayout = (FrameworkElement)GetTemplateChild("RootLayout");

            // 实际内容容器。
            _contentBorder = (ContentControl)GetTemplateChild("ContentBorder");

            // 倒影图片。
            _reflectionImage = (Image)GetTemplateChild("ReflectionImage");

            // 倒影位移。
            _spacingTransform = (TranslateTransform)GetTemplateChild("SpacingTransform");
            _spacingTransform.Y = ReflectionSpacing;

            if (DesignMode.DesignModeEnabled == false)
            {
                rootLayout.LayoutUpdated += RootLayoutChanged;
            }
        }

        private static void ReflectionSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ReflectionPanel)d;
            var value = (double)e.NewValue;

            if (obj._spacingTransform != null)
            {
                obj._spacingTransform.Y = value;
            }
        }

        private async void RootLayoutChanged(object sender, object e)
        {
            try
            {
                // 呈现控件到图像源。
                var contentRender = new RenderTargetBitmap();
                await contentRender.RenderAsync(_contentBorder);

                // 获取图像数据。
                var bgra8 = (await contentRender.GetPixelsAsync()).ToArray();

                // 获取图像高度和宽度。
                var width = contentRender.PixelWidth;
                var height = contentRender.PixelHeight;

                for (var i = 0; i < bgra8.Length; i += 4)
                {
                    // 获取该像素原来的 A 通道。
                    var a = bgra8[i + 3];

                    // 计算该像素的 Y 轴坐标。
                    var y = (i / 4) / width;

                    // 计算新的 A 通道值。
                    bgra8[i + 3] = (byte)(a * y / height);
                }

                var outputBitmap = new WriteableBitmap(width, height);
                bgra8.CopyTo(outputBitmap.PixelBuffer);

                // 设置倒影图片。
                _reflectionImage.Source = outputBitmap;
            }
            catch
            {
            }
        }
    }
}