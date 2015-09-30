using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class DisplayMemoryUsage : Control
    {
        public DisplayMemoryUsage()
        {
            this.DefaultStyleKey = typeof(DisplayMemoryUsage);
        }

        protected override void OnApplyTemplate()
        {
            TextBlock txtMemoryUsage = (TextBlock)GetTemplateChild("txtMemoryUsage");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                string usage = (MemoryManager.AppMemoryUsage / 1024.0 / 1024.0).ToString("f2") + "MB";
                string limit = (MemoryManager.AppMemoryUsageLimit / 1024.0 / 1024.0).ToString("f2") + "MB";
                txtMemoryUsage.Text = usage + "/" + limit;
            };
            timer.Start();
        }
    }
}
