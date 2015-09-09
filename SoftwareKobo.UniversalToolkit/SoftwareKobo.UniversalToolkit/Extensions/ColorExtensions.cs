using System.Globalization;
using Windows.UI;
using Windows.UI.ViewManagement;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// Color 扩展类。
    /// </summary>
    public static class ColorExtensions
    {
        public static Color AccentColor
        {
            get
            {
                return new UISettings().GetColorValue(UIColorType.Accent);
            }
        }

        public static Color FromHex(string hex)
        {
            byte r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
            return FromRgb(r, g, b);
        }

        /// <summary>
        /// 根据颜色的 R，G，B 值返回一个完全不透明的 Color 实例。
        /// </summary>
        /// <param name="r">R 通道的值。</param>
        /// <param name="g">G 通道的值。</param>
        /// <param name="b">B 通道的值。</param>
        /// <returns>Color 实例。</returns>
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return Color.FromArgb(255, r, g, b);
        }
    }
}