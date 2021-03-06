﻿using SoftwareKobo.UniversalToolkit.Controls;
using System.Diagnostics;
using Windows.UI.Xaml;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class DebugSettingsExtensions
    {
        private static FullWindowPopup _displayMemoryPopup;

        private static FullWindowPopup DisplayMemoryPopup
        {
            get
            {
                if (_displayMemoryPopup == null)
                {
                    var memoryUsage = new DisplayMemoryUsage()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Bottom,
                        Margin = new Thickness(0, 0, 0, 48)
                    };
                    _displayMemoryPopup = new FullWindowPopup()
                    {
                        Child = memoryUsage
                    };
                }
                return _displayMemoryPopup;
            }
        }

        /// <summary>
        /// 设置是否显示当前内存使用量。
        /// </summary>
        /// <param name="debugSettings">debug 设置。</param>
        /// <param name="isEnable">是否显示内存使用量。</param>
        /// <example>
        /// public App()
        /// {
        ///     // 仅在你的 App 类继承自 Bootstrapper 下正确。
        ///     this.DebugSettings.EnableDisplayMemoryUsage();
        /// }
        ///
        /// protected override Task OnPreStartAsync(IActivatedEventArgs args, AppStartInfo info)
        /// {
        ///     // 正确使用。
        ///     this.DebugSettings.EnableDisplayMemoryUsage();
        /// }
        /// </example>
        [Conditional("DEBUG")]
        public static void EnableDisplayMemoryUsage(this DebugSettings debugSettings, bool isEnable = true)
        {
            DisplayMemoryPopup.IsOpen = isEnable;
        }
    }
}