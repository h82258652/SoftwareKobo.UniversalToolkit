using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public class WebViewExtensions
    {
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(WebViewExtensions), new PropertyMetadata(null, HtmlChanged));

        public static string GetHtml(WebView obj)
        {
            return (string)obj.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebView obj, string value)
        {
            obj.SetValue(HtmlProperty, value);
        }

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView webView = (WebView)d;
            string value = (string)e.NewValue;

            webView.NavigateToString(value);
        }
    }
}