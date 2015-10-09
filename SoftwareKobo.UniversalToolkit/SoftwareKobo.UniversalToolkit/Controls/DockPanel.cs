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
                return (bool)this.GetValue(LastChildFillProperty);
            }
            set
            {
                this.SetValue(LastChildFillProperty, value);
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

        protected override Size ArrangeOverride(Size finalSize)
        {
            throw new NotImplementedException();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            throw new NotImplementedException();
        }

        private static void DockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement obj = (UIElement)d;
            Dock value = (Dock)e.NewValue;

            if (Enum.IsDefined(typeof(Dock), value) == false)
            {
                throw new ArgumentException("dock is not defined.", nameof(value));
            }

            DockPanel panel = VisualTreeHelper.GetParent(obj) as DockPanel;
            if (panel != null)
            {
                panel.InvalidateMeasure();
            }
        }

        private static void LastChildFillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DockPanel obj = (DockPanel)d;
            obj.InvalidateArrange();
        }
    }
}