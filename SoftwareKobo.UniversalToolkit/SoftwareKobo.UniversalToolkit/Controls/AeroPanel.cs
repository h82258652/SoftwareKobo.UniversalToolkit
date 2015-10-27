using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class AeroPanel : Control
    {
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), typeof(AeroPanel), new PropertyMetadata(null));

        public static readonly DependencyProperty MaskColorProperty = DependencyProperty.Register(nameof(MaskColor), typeof(Color), typeof(AeroPanel), new PropertyMetadata(Colors.Transparent, MaskColorChanged));
        public static readonly DependencyProperty RenderElementProperty = DependencyProperty.Register(nameof(RenderElement), typeof(FrameworkElement), typeof(AeroPanel), new PropertyMetadata(null, RenderElementChanged));
        private byte[] _bytes;

        private CanvasAnimatedControl _canvas;

        private float _dpi;

        private GaussianBlurEffect _effect;

        private int _heightInPixels;

        private Color _maskColor;

        private int _widthInPixels;

        public AeroPanel()
        {
            this.DefaultStyleKey = typeof(AeroPanel);
            this.Loaded += this.AeroPanel_Loaded;
            this.Unloaded += this.AeroPanel_Unloaded;
        }

        public object Content
        {
            get
            {
                return this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        public Color MaskColor
        {
            get
            {
                return (Color)this.GetValue(MaskColorProperty);
            }
            set
            {
                this.SetValue(MaskColorProperty, value);
            }
        }

        public FrameworkElement RenderElement
        {
            get
            {
                return (FrameworkElement)this.GetValue(RenderElementProperty);
            }
            set
            {
                this.SetValue(RenderElementProperty, value);
            }
        }

        public async void UpdateRender()
        {
            if (this._canvas.ReadyToDraw == false)
            {
                return;
            }

            if (this._dpi <= 0)
            {
                return;
            }

            double thisWidth = this.ActualWidth;
            double thisHeight = this.ActualHeight;
            if (thisWidth <= 0 || thisHeight <= 0)
            {
                return;
            }

            if (this.RenderElement == null)
            {
                return;
            }

            try
            {
                float scale = this._dpi / 96;
                int width = (int)(thisWidth * scale);
                int height = (int)(thisHeight * scale);
                GeneralTransform transform = this.TransformToVisual(this.RenderElement);
                Point location = transform.TransformPoint(new Point());
                int left = (int)(location.X * scale);
                int top = (int)(location.Y * scale);

                RenderTargetBitmap bitmap = new RenderTargetBitmap();
                await bitmap.RenderAsync(this.RenderElement);
                byte[] pixels = (await bitmap.GetPixelsAsync()).ToArray();
                int bitmapWidth = bitmap.PixelWidth;
                if (bitmapWidth <= 0)
                {
                    return;
                }

                IList<byte> buffer = new List<byte>(width * height * 4);
                int pixelsLength = pixels.Length;
                for (int y = top; y < top + height; y++)
                {
                    for (int x = left; x < left + width; x++)
                    {
                        int offset = (y * bitmapWidth + x) * 4;
                        if (offset < pixelsLength && offset >= 0)
                        {
                            buffer.Add(pixels[offset]);
                            buffer.Add(pixels[offset + 1]);
                            buffer.Add(pixels[offset + 2]);
                            buffer.Add(pixels[offset + 3]);
                        }
                        else
                        {
                            buffer.Add(255);
                            buffer.Add(255);
                            buffer.Add(255);
                            buffer.Add(0);
                        }
                    }
                }

                this._bytes = buffer.ToArray();
                this._widthInPixels = width;
                this._heightInPixels = height;
            }
            catch
            {
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._canvas = (CanvasAnimatedControl)base.GetTemplateChild("canvas");
            this._canvas.CreateResources += this.Canvas_CreateResources;
            this._canvas.Draw += this.Canvas_Draw;
        }

        private static void MaskColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AeroPanel obj = (AeroPanel)d;
            Color value = (Color)e.NewValue;

            obj._maskColor = value;
        }

        private static void RenderElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AeroPanel obj = (AeroPanel)d;

            FrameworkElement oldValue = (FrameworkElement)e.OldValue;
            if (oldValue != null)
            {
                oldValue.LayoutUpdated -= obj.RenderElement_LayoutUpdated;
            }

            FrameworkElement newValue = (FrameworkElement)e.NewValue;
            if (newValue != null)
            {
                newValue.LayoutUpdated += obj.RenderElement_LayoutUpdated;
            }
        }

        private void AeroPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();
            this._dpi = displayInformation.LogicalDpi;
            displayInformation.DpiChanged += this.DpiChanged;
        }

        private void AeroPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            DisplayInformation.GetForCurrentView().DpiChanged -= this.DpiChanged;

            if (this._canvas != null)
            {
                this._canvas.RemoveFromVisualTree();
                this._canvas = null;
            }
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            this._effect = new GaussianBlurEffect()
            {
                BlurAmount = 5,
                BorderMode = EffectBorderMode.Hard
            };
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            CanvasCommandList cl = new CanvasCommandList(sender);
            using (CanvasDrawingSession clds = cl.CreateDrawingSession())
            {
                if (this._bytes != null)
                {
                    using (CanvasBitmap bitmap = CanvasBitmap.CreateFromBytes(sender, this._bytes, this._widthInPixels, this._heightInPixels, DirectXPixelFormat.B8G8R8A8UIntNormalized, this._dpi))
                    {
                        clds.DrawImage(bitmap);
                    }
                    clds.FillRectangle(0, 0, this._widthInPixels, this._heightInPixels, this._maskColor);
                }
                else
                {
                    clds.FillRectangle(0, 0, (float)sender.Size.Width, (float)sender.Size.Height, this._maskColor);
                }
            }

            this._effect.Source = cl;
            args.DrawingSession.DrawImage(this._effect);
        }

        private void DpiChanged(DisplayInformation sender, object args)
        {
            this._dpi = sender.LogicalDpi;
        }

        private void RenderElement_LayoutUpdated(object sender, object e)
        {
            this.UpdateRender();
        }
    }
}