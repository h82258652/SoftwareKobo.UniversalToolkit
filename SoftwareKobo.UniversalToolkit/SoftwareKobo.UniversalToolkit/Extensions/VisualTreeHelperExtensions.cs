using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// VisualTreeHelper 扩展类。
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        public static IEnumerable<DependencyObject> GetChildren(DependencyObject reference)
        {
            int count = VisualTreeHelper.GetChildrenCount(reference);
            for (int childIndex = 0; childIndex < count; childIndex++)
            {
                yield return VisualTreeHelper.GetChild(reference, childIndex);
            }
        }
    }
}