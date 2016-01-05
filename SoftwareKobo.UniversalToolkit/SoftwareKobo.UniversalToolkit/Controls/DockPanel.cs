using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class DockPanel : Panel
    {
        public static readonly DependencyProperty DockProperty = DependencyProperty.RegisterAttached("Dock", typeof(Dock), typeof(DockPanel), new PropertyMetadata(Dock.Left, DockChanged));

        public static readonly DependencyProperty LastChildFillProperty = DependencyProperty.Register(nameof(LastChildFill), typeof(bool), typeof(DockPanel), new PropertyMetadata(true, LastChildFillChanged));

        public bool LastChildFill
        {
            get
            {
                return (bool)GetValue(LastChildFillProperty);
            }
            set
            {
                SetValue(LastChildFillProperty, value);
            }
        }

        public static Dock GetDock(UIElement obj)
        {
            return (Dock)obj.GetValue(DockProperty);
        }

        public static void SetDock(UIElement obj, Dock value)
        {
            obj.SetValue(DockProperty, value);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            // 表示剩余区域的距离可用区域的边距。
            var left = 0.0d;
            var top = 0.0d;
            var right = 0.0d;
            var bottom = 0.0d;

            var dockedCount = Children.Count - (LastChildFill ? 1 : 0);
            var index = 0;
            foreach (var element in Children)
            {
                var remainingWidth = Math.Max(0.0d, arrangeSize.Width - left - right);
                var remainingHeight = Math.Max(0.0d, arrangeSize.Height - top - bottom);
                var remainingRect = new Rect(left, top, remainingWidth, remainingHeight);

                // 用于判断最后一个元素是否需要 Fill，如果是 Fill，那么则不执行该条件判断内的语句，将剩余空间全布局给最后一个元素。若不是 Fill 则，计算再布局。
                if (index < dockedCount)
                {
                    // 元素所需要的大小。
                    var desiredSize = element.DesiredSize;

                    switch (GetDock(element))
                    {
                        case Dock.Left:
                            left += desiredSize.Width;
                            remainingRect.Width = desiredSize.Width;
                            break;

                        case Dock.Top:
                            top += desiredSize.Height;
                            remainingRect.Height = desiredSize.Height;
                            break;

                        case Dock.Right:
                            right += desiredSize.Width;
                            remainingRect.X = Math.Max(0.0d, arrangeSize.Width - right);
                            remainingRect.Width = desiredSize.Width;
                            break;

                        case Dock.Bottom:

                            bottom += desiredSize.Height;
                            remainingRect.Y = Math.Max(0.0d, arrangeSize.Height - bottom);
                            remainingRect.Height = desiredSize.Height;
                            break;

                        default:
                            break;
                    }
                }

                element.Arrange(remainingRect);
                index++;
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            // 已经使用的宽和高。
            var usedWidth = 0.0d;
            var usedHeight = 0.0d;

            // 所需的最大宽度。
            var maximumWidth = 0.0d;

            // 所需的最大高度。
            var maximumHeight = 0.0d;

            foreach (var element in Children)
            {
                // 计算还有多少空间能给子控件。
                var remainingSize = new Size(Math.Max(0.0d, constraint.Width - usedWidth), Math.Max(0.0d, constraint.Height - usedHeight));
                element.Measure(remainingSize);
                // 子控件需要需要的大小。
                var desiredSize = element.DesiredSize;

                switch (GetDock(element))
                {
                    case Dock.Left:
                    case Dock.Right:
                        maximumHeight = Math.Max(maximumHeight, usedHeight + desiredSize.Height);
                        usedWidth += desiredSize.Width;
                        break;

                    case Dock.Top:
                    case Dock.Bottom:
                        maximumWidth = Math.Max(maximumWidth, usedWidth + desiredSize.Width);
                        usedHeight += desiredSize.Height;
                        break;
                }
            }

            maximumWidth = Math.Max(maximumWidth, usedWidth);
            maximumHeight = Math.Max(maximumHeight, usedHeight);
            return new Size(maximumWidth, maximumHeight);
        }

        private static void DockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (UIElement)d;
            var value = (Dock)e.NewValue;

            if (Enum.IsDefined(typeof(Dock), value) == false)
            {
                throw new ArgumentException("dock is not defined.", nameof(value));
            }

            var panel = VisualTreeHelper.GetParent(obj) as DockPanel;
            if (panel != null)
            {
                panel.InvalidateMeasure();
            }
        }

        private static void LastChildFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (DockPanel)d;
            obj.InvalidateArrange();
        }
    }
}