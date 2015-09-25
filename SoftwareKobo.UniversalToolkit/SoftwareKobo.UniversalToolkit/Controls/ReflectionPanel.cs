using System;
using System.Linq;
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
            this.DefaultStyleKey = typeof(ReflectionPanel);
        }

        public UIElement Content
        {
            get
            {
                return (UIElement)this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        public double ReflectionSpacing
        {
            get
            {
                return (double)this.GetValue(ReflectionSpacingProperty);
            }
            set
            {
                this.SetValue(ReflectionSpacingProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            FrameworkElement rootLayout = (FrameworkElement)this.GetTemplateChild("RootLayout");

            // 实际内容容器。
            this._contentBorder = (ContentControl)this.GetTemplateChild("ContentBorder");

            // 倒影图片。
            this._reflectionImage = (Image)this.GetTemplateChild("ReflectionImage");

            // 倒影位移。
            this._spacingTransform = (TranslateTransform)this.GetTemplateChild("SpacingTransform");
            this._spacingTransform.Y = this.ReflectionSpacing;

            if (DesignMode.DesignModeEnabled == false)
            {
                rootLayout.LayoutUpdated += this.RootLayoutChanged;
            }
        }

        private static void ReflectionSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ReflectionPanel obj = (ReflectionPanel)d;
            double value = (double)e.NewValue;

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
                RenderTargetBitmap contentRender = new RenderTargetBitmap();
                await contentRender.RenderAsync(this._contentBorder);

                // 获取图像数据。
                byte[] bgra8 = (await contentRender.GetPixelsAsync()).ToArray();

                // 获取图像高度和宽度。
                int width = contentRender.PixelWidth;
                int height = contentRender.PixelHeight;

                for (int i = 0; i < bgra8.Length; i += 4)
                {
                    // 获取该像素原来的 A 通道。
                    byte a = bgra8[i + 3];

                    // 计算该像素的 Y 轴坐标。
                    int y = (i / 4) / width;

                    // 计算新的 A 通道值。
                    bgra8[i + 3] = (byte)(a * y / height);
                }

                WriteableBitmap outputBitmap = new WriteableBitmap(width, height);
                bgra8.CopyTo(outputBitmap.PixelBuffer);

                // 设置倒影图片。
                this._reflectionImage.Source = outputBitmap;
            }
            catch
            {
            }
        }
    }
}