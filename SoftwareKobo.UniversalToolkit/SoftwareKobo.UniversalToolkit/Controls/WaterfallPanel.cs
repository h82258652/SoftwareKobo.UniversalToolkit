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
                return (int)this.GetValue(ColumnCountProperty);
            }
            set
            {
                this.SetValue(ColumnCountProperty, value);
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
            List<double> columnsLength = new List<double>(new double[this.ColumnCount]);

            if (this.Orientation == Orientation.Vertical)
            {
                // 每个流的宽度。
                double itemWidth = finalSize.Width / this.ColumnCount;

                foreach (UIElement element in this.Children)
                {
                    // 子元素的大小。
                    Size elementSize = element.DesiredSize;

                    // 最短流的索引。
                    int minIndex = columnsLength.IndexOf(columnsLength.Min());

                    Rect elementRect = new Rect(new Point(itemWidth * minIndex, columnsLength[minIndex]), new Size(itemWidth, elementSize.Height));

                    element.Arrange(elementRect);

                    // 将该元素的高度追加到最短流上。
                    columnsLength[minIndex] += elementSize.Height;
                }
            }
            else
            {
                double itemHeight = finalSize.Height / this.ColumnCount;

                foreach (UIElement element in this.Children)
                {
                    Size elementSize = element.DesiredSize;
                    int minIndex = columnsLength.IndexOf(columnsLength.Min());
                    Rect elementRect = new Rect(new Point(columnsLength[minIndex], itemHeight * minIndex), new Size(elementSize.Width, itemHeight));
                    element.Arrange(elementRect);
                    columnsLength[minIndex] += elementSize.Width;
                }
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            List<double> columnsLength = new List<double>(new double[this.ColumnCount]);

            if (this.Orientation == Orientation.Vertical)
            {
                // 每个流的宽度。
                double itemWidth = availableSize.Width / this.ColumnCount;

                // 子元素的大小。
                Size elementMeasureSize = new Size(itemWidth, double.PositiveInfinity);

                foreach (UIElement element in this.Children)
                {
                    // 测量子元素。
                    element.Measure(elementMeasureSize);

                    // 子元素测量结果。
                    Size elementSize = element.DesiredSize;

                    // 最短流的索引。
                    int minIndex = columnsLength.IndexOf(columnsLength.Min());

                    // 将该元素的高度追加到最短流上。
                    columnsLength[minIndex] += elementSize.Height;
                }

                return new Size(availableSize.Width, columnsLength.Max());
            }
            else
            {
                double itemHeight = availableSize.Height / this.ColumnCount;
                Size elementMeasureSize = new Size(double.PositiveInfinity, itemHeight);

                foreach (UIElement element in this.Children)
                {
                    element.Measure(elementMeasureSize);
                    Size elementSize = element.DesiredSize;
                    int minIndex = columnsLength.IndexOf(columnsLength.Min());
                    columnsLength[minIndex] += elementSize.Width;
                }

                return new Size(columnsLength.Max(), availableSize.Height);
            }
        }

        private static void ColumnCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WaterfallPanel obj = (WaterfallPanel)d;
            int value = (int)e.NewValue;

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            obj.InvalidateMeasure();
        }

        private static void OrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WaterfallPanel obj = (WaterfallPanel)d;
            Orientation value = (Orientation)e.NewValue;

            if (Enum.IsDefined(typeof(Orientation), value) == false)
            {
                throw new ArgumentException("orientation is not defined.", nameof(value));
            }

            obj.InvalidateMeasure();
        }
    }
}