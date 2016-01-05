using Microsoft.Xaml.Interactivity;
using SoftwareKobo.UniversalToolkit.Extensions;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace SoftwareKobo.UniversalToolkit.Behaviors
{
    [TypeConstraint(typeof(ListViewBase))]
    public sealed class ListViewBaseScrollBehavior : DependencyObject, IBehavior
    {
        private ScrollBar _horizontalBar;

        private ScrollBar _verticalBar;

        public event EventHandler ScrollDown;

        public event EventHandler ScrollLeft;

        public event EventHandler ScrollRight;

        public event EventHandler ScrollUp;

        public DependencyObject AssociatedObject
        {
            get;
            private set;
        }

        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            var dataView = AssociatedObject as ListViewBase;
            if (dataView != null)
            {
                var scrollViewer = dataView.GetDescendantsOfType<ScrollViewer>().FirstOrDefault();
                if (scrollViewer != null)
                {
                    AttachScrollBar(scrollViewer);
                }
                else
                {
                    RoutedEventHandler loadedHandler = null;
                    loadedHandler = (sender, e) =>
                    {
                        scrollViewer = dataView.GetDescendantsOfType<ScrollViewer>().FirstOrDefault();
                        AttachScrollBar(scrollViewer);
                        dataView.Loaded -= loadedHandler;
                    };
                    dataView.Loaded += loadedHandler;
                }
            }
        }

        public void Detach()
        {
            if (_horizontalBar != null)
            {
                _horizontalBar.ValueChanged -= HorizontalBar_ValueChanged;
            }
            if (_verticalBar != null)
            {
                _verticalBar.ValueChanged -= VerticalBar_ValueChanged;
            }
            AssociatedObject = null;
        }

        private void AttachScrollBar(ScrollViewer scrollViewer)
        {
            var scrollBars = scrollViewer.GetDescendantsOfType<ScrollBar>().ToList();
            var horizontalBar = scrollBars.FirstOrDefault(temp => temp.Orientation == Orientation.Horizontal);
            if (horizontalBar != null)
            {
                _horizontalBar = horizontalBar;
                _horizontalBar.ValueChanged += HorizontalBar_ValueChanged;
            }
            var verticalBar = scrollBars.FirstOrDefault(temp => temp.Orientation == Orientation.Vertical);
            if (verticalBar != null)
            {
                _verticalBar = verticalBar;
                _verticalBar.ValueChanged += VerticalBar_ValueChanged;
            }
        }

        private void HorizontalBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                if (ScrollRight != null)
                {
                    ScrollRight(AssociatedObject, EventArgs.Empty);
                }
            }
            else if (e.NewValue < e.OldValue)
            {
                if (ScrollLeft != null)
                {
                    ScrollLeft(AssociatedObject, EventArgs.Empty);
                }
            }
        }

        private void VerticalBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                if (ScrollDown != null)
                {
                    ScrollDown(AssociatedObject, EventArgs.Empty);
                }
            }
            else if (e.NewValue < e.OldValue)
            {
                if (ScrollUp != null)
                {
                    ScrollUp(AssociatedObject, EventArgs.Empty);
                }
            }
        }
    }
}