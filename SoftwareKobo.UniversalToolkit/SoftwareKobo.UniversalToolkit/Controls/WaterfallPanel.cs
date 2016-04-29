using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class WaterfallPanel : Panel
    {
        public static readonly DependencyProperty ColumnCountProperty = DependencyProperty.Register(nameof(ColumnCount), typeof(int), typeof(WaterfallPanel), new PropertyMetadata(1, ColumnCountChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(WaterfallPanel), new PropertyMetadata(Orientation.Vertical, OrientationChanged));

        public int ColumnCount
        {
            get
            {
                return (int)GetValue(ColumnCountProperty);
            }
            set
            {
                SetValue(ColumnCountProperty, value);
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
            var columnsLength = new List<double>(new double[ColumnCount]);

            if (Orientation == Orientation.Vertical)
            {
                // 每个流的宽度。
                var itemWidth = finalSize.Width / ColumnCount;

                foreach (var element in Children)
                {
                    // 子元素的大小。
                    var elementSize = element.DesiredSize;

                    // 最短流的索引。
                    var minIndex = columnsLength.IndexOf(columnsLength.Min());

                    var elementRect = new Rect(new Point(itemWidth * minIndex, columnsLength[minIndex]), new Size(itemWidth, elementSize.Height));

                    element.Arrange(elementRect);

                    // 将该元素的高度追加到最短流上。
                    columnsLength[minIndex] += elementSize.Height;
                }
            }
            else
            {
                var itemHeight = finalSize.Height / ColumnCount;

                foreach (var element in Children)
                {
                    var elementSize = element.DesiredSize;
                    var minIndex = columnsLength.IndexOf(columnsLength.Min());
                    var elementRect = new Rect(new Point(columnsLength[minIndex], itemHeight * minIndex), new Size(elementSize.Width, itemHeight));
                    element.Arrange(elementRect);
                    columnsLength[minIndex] += elementSize.Width;
                }
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var columnsLength = new List<double>(new double[ColumnCount]);

            if (Orientation == Orientation.Vertical)
            {
                // 每个流的宽度。
                var itemWidth = availableSize.Width / ColumnCount;

                // 子元素的大小。
                var elementMeasureSize = new Size(itemWidth, double.PositiveInfinity);

                foreach (var element in Children)
                {
                    // 测量子元素。
                    element.Measure(elementMeasureSize);

                    // 子元素测量结果。
                    var elementSize = element.DesiredSize;

                    // 最短流的索引。
                    var minIndex = columnsLength.IndexOf(columnsLength.Min());

                    // 将该元素的高度追加到最短流上。
                    columnsLength[minIndex] += elementSize.Height;
                }

                return new Size(availableSize.Width, columnsLength.Max());
            }
            else
            {
                var itemHeight = availableSize.Height / ColumnCount;
                var elementMeasureSize = new Size(double.PositiveInfinity, itemHeight);

                foreach (var element in Children)
                {
                    element.Measure(elementMeasureSize);
                    var elementSize = element.DesiredSize;
                    var minIndex = columnsLength.IndexOf(columnsLength.Min());
                    columnsLength[minIndex] += elementSize.Width;
                }

                return new Size(columnsLength.Max(), availableSize.Height);
            }
        }

        private static void ColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (WaterfallPanel)d;
            var value = (int)e.NewValue;

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            obj.InvalidateMeasure();
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (WaterfallPanel)d;
            var value = (Orientation)e.NewValue;

            if (Enum.IsDefined(typeof(Orientation), value) == false)
            {
                throw new ArgumentException("orientation is not defined.", nameof(value));
            }

            obj.InvalidateMeasure();
        }
    }
}