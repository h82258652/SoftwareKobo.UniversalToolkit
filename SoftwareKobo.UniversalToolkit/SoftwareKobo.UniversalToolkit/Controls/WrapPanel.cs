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
                return (double)GetValue(ItemHeightProperty);
            }
            set
            {
                SetValue(ItemHeightProperty, value);
            }
        }

        public double ItemWidth
        {
            get
            {
                return (double)GetValue(ItemWidthProperty);
            }
            set
            {
                SetValue(ItemWidthProperty, value);
            }
        }

        public Orientation Orientation
        {
            get
            {
                return (Orientation)GetValue(OrientationProperty);
            }
            set
            {
                SetValue(OrientationProperty, value);
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var orientation = Orientation;
            var lineSize = new OrientedSize(orientation);
            var maximumSize = new OrientedSize(orientation, finalSize.Width, finalSize.Height);

            var itemWidth = ItemWidth;
            var itemHeight = ItemHeight;
            var hasFixedWidth = double.IsNaN(itemWidth) == false;
            var hasFixedHeight = double.IsNaN(itemHeight) == false;
            var indirectOffset = 0.0d;
            var directDelta = (orientation == Orientation.Horizontal) ? (hasFixedWidth ? (double?)itemWidth : null) : (hasFixedHeight ? (double?)itemHeight : null);

            var children = Children;
            var count = children.Count;
            var lineStart = 0;
            for (var lineEnd = 0; lineEnd < count; lineEnd++)
            {
                var element = children[lineEnd];

                var elementSize = new OrientedSize(orientation, hasFixedWidth ? itemWidth : element.DesiredSize.Width, hasFixedHeight ? itemHeight : element.DesiredSize.Height);

                if (lineSize.Direct + elementSize.Direct > maximumSize.Direct)
                {
                    ArrangeLine(lineStart, lineEnd, directDelta, indirectOffset, lineSize.Indirect);

                    indirectOffset += lineSize.Indirect;
                    lineSize = elementSize;

                    if (elementSize.Direct > maximumSize.Direct)
                    {
                        ArrangeLine(lineEnd, ++lineEnd, directDelta, indirectOffset, elementSize.Indirect);

                        indirectOffset += lineSize.Indirect;
                        lineSize = new OrientedSize(orientation);
                    }

                    lineStart = lineEnd;
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }

            if (lineStart < count)
            {
                ArrangeLine(lineStart, count, directDelta, indirectOffset, lineSize.Indirect);
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var orientation = Orientation;
            var lineSize = new OrientedSize(orientation);
            var totalSize = new OrientedSize(orientation);
            var maximumSize = new OrientedSize(orientation, constraint.Width, constraint.Height);

            var itemWidth = ItemWidth;
            var itemHeight = ItemHeight;
            var hasFixedWidth = double.IsNaN(itemWidth) == false;
            var hasFixedHeihgt = double.IsNaN(itemHeight) == false;
            var itemSize = new Size(hasFixedWidth ? itemWidth : constraint.Width, hasFixedHeihgt ? itemHeight : constraint.Height);

            foreach (var element in Children)
            {
                element.Measure(itemSize);
                var elementSize = new OrientedSize(orientation, hasFixedWidth ? itemWidth : element.DesiredSize.Width, hasFixedHeihgt ? itemHeight : element.DesiredSize.Height);

                if (lineSize.Direct + elementSize.Direct > maximumSize.Direct)
                {
                    totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
                    totalSize.Indirect += lineSize.Indirect;

                    lineSize = elementSize;

                    if (elementSize.Direct > maximumSize.Direct)
                    {
                        totalSize.Direct = Math.Max(elementSize.Direct, totalSize.Direct);
                        totalSize.Indirect += elementSize.Indirect;

                        lineSize = new OrientedSize(orientation);
                    }
                }
                else
                {
                    lineSize.Direct += elementSize.Direct;
                    lineSize.Indirect = Math.Max(lineSize.Indirect, elementSize.Indirect);
                }
            }

            totalSize.Direct = Math.Max(lineSize.Direct, totalSize.Direct);
            totalSize.Indirect += lineSize.Indirect;

            return new Size(totalSize.Width, totalSize.Height);
        }

        private static bool IsItemWidthHeightValid(double value)
        {
            return double.IsNaN(value) || (value >= 0.0d && double.IsPositiveInfinity(value) == false);
        }

        private static void ItemWidthOrHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (WrapPanel)d;
            var value = (double)e.NewValue;

            if (IsItemWidthHeightValid(value) == false)
            {
                throw new ArgumentException("Invalid item width or height", nameof(value));
            }

            obj.InvalidateMeasure();
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (WrapPanel)d;
            var value = (Orientation)e.NewValue;

            if (Enum.IsDefined(typeof(Orientation), value) == false)
            {
                throw new ArgumentException($"{nameof(Orientation)} is not defined", nameof(value));
            }

            obj.InvalidateMeasure();
        }

        private void ArrangeLine(int lineStart, int lineEnd, double? directDelta, double indirectOffset, double indirectGrowth)
        {
            var directOffset = 0.0d;

            var orientation = Orientation;
            var isHorizontal = orientation == Orientation.Horizontal;

            var children = Children;
            for (var index = lineStart; index < lineEnd; index++)
            {
                var element = children[index];
                var elementSize = new OrientedSize(orientation, element.DesiredSize.Width, element.DesiredSize.Height);

                var directGrowth = directDelta != null ? directDelta.Value : elementSize.Direct;

                var bounds = isHorizontal ? new Rect(directOffset, indirectOffset, directGrowth, indirectGrowth) : new Rect(indirectOffset, directOffset, indirectGrowth, directGrowth);
                element.Arrange(bounds);

                directOffset += directGrowth;
            }
        }
    }
}