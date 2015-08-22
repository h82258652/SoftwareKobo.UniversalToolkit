using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Helpers
{
    public static class ScreenResolutionHelper
    {
        public static async Task<int> GetWidthAsync()
        {
            WebView webView = new WebView(WebViewExecutionMode.SeparateThread);
            int width;
            int.TryParse(await webView.InvokeScriptAsync("eval", new string[] { "window.screen.width.toString()" }), out width);
            return width;
        }

        public static async Task<int> GetHeightAsync()
        {
            WebView webView = new WebView(WebViewExecutionMode.SeparateThread);
            int height;
            int.TryParse(await webView.InvokeScriptAsync("eval", new string[] { "window.screen.height.toString()" }), out height);
            return height;
        }
    }
}