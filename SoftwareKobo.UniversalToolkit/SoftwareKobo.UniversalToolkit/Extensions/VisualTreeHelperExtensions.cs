using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// VisualTreeHelper 扩展类。
    /// </summary>
    public static class VisualTreeHelperExtensions
    {
        /// <summary>
        /// 获取该节点在可视树上的所有祖宗。
        /// </summary>
        /// <param name="reference">该节点。</param>
        /// <returns>所有祖宗。</returns>
        /// <exception cref="ArgumentNullException"><c>reference</c> 为空。</exception>
        public static IEnumerable<DependencyObject> GetAncestors(this DependencyObject reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var parent = VisualTreeHelper.GetParent(reference);
            while (parent != null)
            {
                yield return parent;
                parent = VisualTreeHelper.GetParent(parent);
            }
        }

        /// <summary>
        /// 获取该节点在可视树上符合类型的所有祖宗。
        /// </summary>
        /// <typeparam name="T">匹配类型。</typeparam>
        /// <param name="reference">该节点。</param>
        /// <returns>符合类型的所有祖宗。</returns>
        /// <exception cref="ArgumentNullException"><c>reference</c> 为空。</exception>
        public static IEnumerable<T> GetAncestorsOfType<T>(this DependencyObject reference) where T : DependencyObject
        {
            return reference.GetAncestors().OfType<T>();
        }

        /// <summary>
        /// 获取该控件在可视树中的子级控件。
        /// </summary>
        /// <param name="reference">该控件</param>
        /// <returns>子级控件。</returns>
        public static IEnumerable<DependencyObject> GetChildren(this DependencyObject reference)
        {
            var count = VisualTreeHelper.GetChildrenCount(reference);
            for (var childIndex = 0; childIndex < count; childIndex++)
            {
                yield return VisualTreeHelper.GetChild(reference, childIndex);
            }
        }

        /// <summary>
        /// 获取该节点在可视树上的所有后代。
        /// </summary>
        /// <param name="reference">该节点。</param>
        /// <returns>所有后代。</returns>
        /// <exception cref="ArgumentNullException"><c>reference</c> 为空。</exception>
        public static IEnumerable<DependencyObject> GetDescendants(this DependencyObject reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }

            var descendants = new List<DependencyObject>();

            var children = reference.GetChildren();
            foreach (var child in children)
            {
                descendants.Add(child);
                descendants.AddRange(GetDescendants(child));
            }

            return descendants;
        }

        /// <summary>
        /// 获取该节点在可视树上符合类型的所有祖宗。
        /// </summary>
        /// <typeparam name="T">匹配类型。</typeparam>
        /// <param name="reference">该节点。</param>
        /// <returns>符合类型的所有后代。</returns>
        /// <exception cref="ArgumentNullException"><c>reference</c> 为空。</exception>
        public static IEnumerable<T> GetDescendantsOfType<T>(this DependencyObject reference) where T : DependencyObject
        {
            return reference.GetDescendants().OfType<T>();
        }
    }
}