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
            this.DefaultStyleKey = typeof(DisplayMemoryUsage);
        }

        protected override void OnApplyTemplate()
        {
            this._txtMemoryUsage = (TextBlock)GetTemplateChild("txtMemoryUsage");
            DispatcherTimer timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        [DebuggerNonUserCode]
        private void Timer_Tick(object sender, object e)
        {
            string usage = (MemoryManager.AppMemoryUsage / 1024.0 / 1024.0).ToString("f2") + "MB";
            string limit = (MemoryManager.AppMemoryUsageLimit / 1024.0 / 1024.0).ToString("f2") + "MB";
            this._txtMemoryUsage.Text = usage + "/" + limit;
        }
    }
}