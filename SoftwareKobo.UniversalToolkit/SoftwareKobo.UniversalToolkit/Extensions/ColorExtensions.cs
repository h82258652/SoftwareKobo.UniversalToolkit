using System.Globalization;
using Windows.UI;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class ColorExtensions
    {
        public static Color FromRgb(byte r, byte g, byte b)
        {
            return Color.FromArgb(255, r, g, b);
        }

        public static Color FromHex(string hex)
        {
            byte r = byte.Parse(hex.Substring(1, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(3, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(5, 2), NumberStyles.HexNumber);
            return FromRgb(r, g, b);
        }
    }
}