using System;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class ListViewBaseExtensions
    {
        public static void ScrollIntoViewSmoothly(this ListViewBase listViewBase, object item)
        {
            ScrollIntoViewSmoothly(listViewBase, item, ScrollIntoViewAlignment.Default);
        }

        public static void ScrollIntoViewSmoothly(this ListViewBase listViewBase, object item, ScrollIntoViewAlignment alignment)
        {
            if (listViewBase == null)
            {
                throw new ArgumentNullException(nameof(listViewBase));
            }

            ScrollViewer scrollViewer = listViewBase.GetDescendantsOfType<ScrollViewer>().First();

            double originHorizontalOffset = scrollViewer.HorizontalOffset;
            double originVerticalOffset = scrollViewer.VerticalOffset;

            EventHandler<object> handler = null;
            handler = delegate
            {
                listViewBase.LayoutUpdated -= handler;

                double targetHorizontalOffset = scrollViewer.HorizontalOffset;
                double targetVerticalOffset = scrollViewer.VerticalOffset;

                EventHandler<ScrollViewerViewChangedEventArgs> scrollHandler = null;
                scrollHandler = (sender, e) =>
                {
                    scrollViewer.ViewChanged -= scrollHandler;

                    scrollViewer.ChangeView(targetHorizontalOffset, targetVerticalOffset, null);
                };
                scrollViewer.ViewChanged += scrollHandler; ;

                scrollViewer.ChangeView(originHorizontalOffset, originVerticalOffset, null, true);
            };
            listViewBase.LayoutUpdated += handler;

            listViewBase.ScrollIntoView(item, alignment);
        }
    }
}