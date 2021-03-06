﻿using System;
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

            var scrollViewer = listViewBase.GetDescendantsOfType<ScrollViewer>().First();

            var originHorizontalOffset = scrollViewer.HorizontalOffset;
            var originVerticalOffset = scrollViewer.VerticalOffset;

            EventHandler<object> layoutUpdatedHandler = null;
            layoutUpdatedHandler = delegate
            {
                listViewBase.LayoutUpdated -= layoutUpdatedHandler;

                var targetHorizontalOffset = scrollViewer.HorizontalOffset;
                var targetVerticalOffset = scrollViewer.VerticalOffset;

                EventHandler<ScrollViewerViewChangedEventArgs> scrollHandler = null;
                scrollHandler = (sender, e) =>
                {
                    scrollViewer.ViewChanged -= scrollHandler;

                    scrollViewer.ChangeView(targetHorizontalOffset, targetVerticalOffset, null);
                };
                scrollViewer.ViewChanged += scrollHandler; ;

                scrollViewer.ChangeView(originHorizontalOffset, originVerticalOffset, null, true);
            };
            listViewBase.LayoutUpdated += layoutUpdatedHandler;

            listViewBase.ScrollIntoView(item, alignment);
        }
    }
}