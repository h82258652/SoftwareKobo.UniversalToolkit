using System.Diagnostics;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public sealed class DisplayMemoryUsage : Control
    {
        private TextBlock _txtMemoryUsage;

        public DisplayMemoryUsage()
        {
            DefaultStyleKey = typeof(DisplayMemoryUsage);
        }

        protected override void OnApplyTemplate()
        {
            _txtMemoryUsage = (TextBlock)GetTemplateChild("txtMemoryUsage");
            var timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        [DebuggerNonUserCode]
        private void Timer_Tick(object sender, object e)
        {
            var usage = (MemoryManager.AppMemoryUsage / 1024.0 / 1024.0).ToString("f2") + "MB";
            var limit = (MemoryManager.AppMemoryUsageLimit / 1024.0 / 1024.0).ToString("f2") + "MB";
            _txtMemoryUsage.Text = usage + "/" + limit;
        }
    }
}