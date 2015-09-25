using System.Linq;
using Windows.Devices.Input;
using Windows.Graphics.Display;

namespace SoftwareKobo.UniversalToolkit.Utils
{
    /// <summary>
    /// 屏幕分辨率。
    /// </summary>
    public static class ScreenResolution
    {
        /// <summary>
        /// 获取屏幕高度。
        /// </summary>
        /// <remarks>包含虚拟导航栏高度（如果存在虚拟导航栏）。</remarks>
        public static int Height
        {
            get
            {
                var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
                var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                return (int)(rect.Height * scale);
            }
        }

        /// <summary>
        /// 获取屏幕宽度。
        /// </summary>
        public static int Width
        {
            get
            {
                var rect = PointerDevice.GetPointerDevices().Last().ScreenRect;
                var scale = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                return (int)(rect.Width * scale);
            }
        }
    }
}