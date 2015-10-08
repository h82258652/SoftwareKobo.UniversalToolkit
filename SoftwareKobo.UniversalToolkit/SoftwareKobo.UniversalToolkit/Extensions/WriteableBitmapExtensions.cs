using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    public static class WriteableBitmapExtensions
    {
        public static Color GetPixel(this WriteableBitmap bitmap, int x, int y)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            if (x < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
            if (y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            int width = bitmap.PixelWidth;
            if (x >= width)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            int height = bitmap.PixelHeight;
            if (y >= height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            using (var stream = bitmap.PixelBuffer.AsStream())
            {
                int offset = y * width * 4 + x * 4;
                stream.Seek(offset, SeekOrigin.Begin);
                byte b = (byte)stream.ReadByte();
                byte g = (byte)stream.ReadByte();
                byte r = (byte)stream.ReadByte();
                byte a = (byte)stream.ReadByte();
                return Color.FromArgb(a, r, g, b);
            }
        }

        public static void SetPixel(this WriteableBitmap bitmap, int x, int y, Color color)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException(nameof(bitmap));
            }

            if (x < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }
            if (y < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            int width = bitmap.PixelWidth;
            if (x >= width)
            {
                throw new ArgumentOutOfRangeException(nameof(x));
            }

            int height = bitmap.PixelHeight;
            if (y >= height)
            {
                throw new ArgumentOutOfRangeException(nameof(y));
            }

            using (var stream = bitmap.PixelBuffer.AsStream())
            {
                int offset = y * width * 4 + x * 4;
                stream.Seek(offset, SeekOrigin.Begin);
                stream.WriteByte(color.B);
                stream.WriteByte(color.G);
                stream.WriteByte(color.R);
                stream.WriteByte(color.A);
            }
            bitmap.Invalidate();
        }
    }
}