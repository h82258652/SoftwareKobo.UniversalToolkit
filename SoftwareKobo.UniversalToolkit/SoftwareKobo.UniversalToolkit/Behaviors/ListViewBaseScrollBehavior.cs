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
    public sealed class ListViewBaseScrollBehavior : IBehavior
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
            this.AssociatedObject = associatedObject;
            ListViewBase dataView = this.AssociatedObject as ListViewBase;
            if (dataView != null)
            {
                ScrollViewer scrollViewer = dataView.GetDescendantsOfType<ScrollViewer>().FirstOrDefault();
                if (scrollViewer != null)
                {
                    var scrollBars = scrollViewer.GetDescendantsOfType<ScrollBar>().ToList();
                    ScrollBar horizontalBar = scrollBars.FirstOrDefault(temp => temp.Orientation == Orientation.Horizontal);
                    if (horizontalBar != null)
                    {
                        this._horizontalBar = horizontalBar;
                        this._horizontalBar.ValueChanged += this.HorizontalBar_ValueChanged;
                    }
                    ScrollBar verticalBar = scrollBars.FirstOrDefault(temp => temp.Orientation == Orientation.Vertical);
                    if (verticalBar != null)
                    {
                        this._verticalBar = verticalBar;
                        this._verticalBar.ValueChanged += this.VerticalBar_ValueChanged;
                    }
                }
            }
        }

        public void Detach()
        {
            if (this._horizontalBar != null)
            {
                this._horizontalBar.ValueChanged -= this.HorizontalBar_ValueChanged;
            }
            if (this._verticalBar != null)
            {
                this._verticalBar.ValueChanged -= this.VerticalBar_ValueChanged;
            }
            this.AssociatedObject = null;
        }

        private void HorizontalBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                if (this.ScrollRight != null)
                {
                    this.ScrollRight(this.AssociatedObject, EventArgs.Empty);
                }
            }
            else if (e.NewValue < e.OldValue)
            {
                if (this.ScrollLeft != null)
                {
                    this.ScrollLeft(this.AssociatedObject, EventArgs.Empty);
                }
            }
        }

        private void VerticalBar_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > e.OldValue)
            {
                if (this.ScrollDown != null)
                {
                    this.ScrollDown(this.AssociatedObject, EventArgs.Empty);
                }
            }
            else if (e.NewValue < e.OldValue)
            {
                if (this.ScrollUp != null)
                {
                    this.ScrollUp(this.AssociatedObject, EventArgs.Empty);
                }
            }
        }
    }
}