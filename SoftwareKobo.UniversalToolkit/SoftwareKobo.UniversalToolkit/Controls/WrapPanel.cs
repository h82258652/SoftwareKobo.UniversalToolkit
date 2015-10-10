using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class WrapPanel : Panel
    {
        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(WrapPanel), new PropertyMetadata(double.NaN, ItemWidthOrHeightChanged));

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(WrapPanel), new PropertyMetadata(double.NaN, ItemWidthOrHeightChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(WrapPanel), new PropertyMetadata(Orientation.Horizontal, OrientationChanged));

        public double ItemHeight
        {
            get
            {
                return (double)this.GetValue(ItemHeightProperty);
            }
            set
            {
                this.SetValue(ItemHeightProperty, value);
            }
        }

        public double ItemWidth
        {
            get
            {
                return (double)this.GetValue(ItemWidthProperty);
            }
            set
            {
                this.SetValue(ItemWidthProperty, value);
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            throw new NotImplementedException();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Orientation orientation = this.Orientation;


            throw new NotImplementedException();
        }

        private static bool IsItemWidthHeightValid(double value)
        {
            return double.IsNaN(value) || (value >= 0.0d && double.IsPositiveInfinity(value) == false);
        }

        private static void ItemWidthOrHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapPanel obj = (WrapPanel)d;
            double value = (double)e.NewValue;

            if (IsItemWidthHeightValid(value) == false)
            {
                throw new ArgumentException("Invalid item width or height", nameof(value));
            }

            obj.InvalidateMeasure();
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WrapPanel obj = (WrapPanel)d;
            Orientation value = (Orientation)e.NewValue;

            if (Enum.IsDefined(typeof(Orientation), value) == false)
            {
                throw new ArgumentException($"{nameof(Orientation)} is not defined", nameof(value));
            }

            obj.InvalidateMeasure();
        }
    }
}