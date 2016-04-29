using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    [ContentProperty(Name = nameof(Content))]
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
            DefaultStyleKey = typeof(AeroPanel);
            Loaded += AeroPanel_Loaded;
            Unloaded += AeroPanel_Unloaded;
        }

        public object Content
        {
            get
            {
                return GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        public Color MaskColor
        {
            get
            {
                return (Color)GetValue(MaskColorProperty);
            }
            set
            {
                SetValue(MaskColorProperty, value);
            }
        }

        public FrameworkElement RenderElement
        {
            get
            {
                return (FrameworkElement)GetValue(RenderElementProperty);
            }
            set
            {
                SetValue(RenderElementProperty, value);
            }
        }

        public async void UpdateRender()
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (_canvas.ReadyToDraw == false)
            {
                return;
            }

            if (_dpi <= 0)
            {
                return;
            }

            var thisWidth = ActualWidth;
            var thisHeight = ActualHeight;
            if (thisWidth <= 0 || thisHeight <= 0)
            {
                return;
            }

            if (RenderElement == null)
            {
                return;
            }

            try
            {
                var scale = _dpi / 96;
                var width = (int)(thisWidth * scale);
                var height = (int)(thisHeight * scale);
                var transform = TransformToVisual(RenderElement);
                var location = transform.TransformPoint(new Point());
                var left = (int)(location.X * scale);
                var top = (int)(location.Y * scale);

                var bitmap = new RenderTargetBitmap();
                await bitmap.RenderAsync(RenderElement);
                var pixels = (await bitmap.GetPixelsAsync()).ToArray();
                var bitmapWidth = bitmap.PixelWidth;
                if (bitmapWidth <= 0)
                {
                    return;
                }

                IList<byte> buffer = new List<byte>(width * height * 4);
                var pixelsLength = pixels.Length;
                for (var y = top; y < top + height; y++)
                {
                    for (var x = left; x < left + width; x++)
                    {
                        var offset = (y * bitmapWidth + x) * 4;
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

                _bytes = buffer.ToArray();
                _widthInPixels = width;
                _heightInPixels = height;
            }
            catch
            {
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _canvas = (CanvasAnimatedControl)GetTemplateChild("canvas");
            _canvas.CreateResources += Canvas_CreateResources;
            _canvas.Draw += Canvas_Draw;
        }

        private static void MaskColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (AeroPanel)d;
            var value = (Color)e.NewValue;

            obj._maskColor = value;
        }

        private static void RenderElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (AeroPanel)d;

            var oldValue = (FrameworkElement)e.OldValue;
            if (oldValue != null)
            {
                oldValue.LayoutUpdated -= obj.RenderElement_LayoutUpdated;
            }

            var newValue = (FrameworkElement)e.NewValue;
            if (newValue != null)
            {
                newValue.LayoutUpdated += obj.RenderElement_LayoutUpdated;
            }
        }

        private void AeroPanel_Loaded(object sender, RoutedEventArgs e)
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            _dpi = displayInformation.LogicalDpi;
            displayInformation.DpiChanged += DpiChanged;
        }

        private void AeroPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            DisplayInformation.GetForCurrentView().DpiChanged -= DpiChanged;

            if (_canvas != null)
            {
                _canvas.RemoveFromVisualTree();
                _canvas = null;
            }
        }

        private void Canvas_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            _effect = new GaussianBlurEffect()
            {
                BlurAmount = 5,
                BorderMode = EffectBorderMode.Hard
            };
        }

        private void Canvas_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            var cl = new CanvasCommandList(sender);
            using (var clds = cl.CreateDrawingSession())
            {
                if (_bytes != null)
                {
                    using (var bitmap = CanvasBitmap.CreateFromBytes(sender, _bytes, _widthInPixels, _heightInPixels, DirectXPixelFormat.B8G8R8A8UIntNormalized, _dpi))
                    {
                        clds.DrawImage(bitmap);
                    }
                    clds.FillRectangle(0, 0, _widthInPixels, _heightInPixels, _maskColor);
                }
                else
                {
                    clds.FillRectangle(0, 0, (float)sender.Size.Width, (float)sender.Size.Height, _maskColor);
                }
            }

            _effect.Source = cl;
            args.DrawingSession.DrawImage(_effect);
        }

        private void DpiChanged(DisplayInformation sender, object args)
        {
            _dpi = sender.LogicalDpi;
        }

        private void RenderElement_LayoutUpdated(object sender, object e)
        {
            UpdateRender();
        }
    }
}