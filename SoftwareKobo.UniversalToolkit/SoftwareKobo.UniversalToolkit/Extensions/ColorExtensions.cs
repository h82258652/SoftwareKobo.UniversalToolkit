using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// Color 扩展类。
    /// </summary>
    public static class ColorExtensions
    {
        private static bool _hadInitAccentColorChanged = false;

        private static IDictionary<string, Color> _knownColors;

        /// <summary>
        /// 用户主题色发生了变化。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1009")]
        public static event EventHandler<Color> AccentColorChanged
        {
            add
            {
                InitAccentColorChanged();
                _accentColorChanged += value;
            }
            remove
            {
                _accentColorChanged -= value;
            }
        }

        private static event EventHandler<Color> _accentColorChanged;

        /// <summary>
        /// 获取用户主题色。
        /// </summary>
        public static Color AccentColor
        {
            get
            {
                return (Color)Application.Current.Resources["SystemAccentColor"];
            }
        }

        /// <summary>
        /// 获取 <see cref="Colors"/> 中已经声明的 <see cref="Color"/>。
        /// </summary>
        public static IDictionary<string, Color> KnownColors
        {
            get
            {
                if (_knownColors == null)
                {
                    _knownColors = new Dictionary<string, Color>();
                    foreach (var property in typeof(Colors).GetRuntimeProperties())
                    {
                        string colorName = property.Name;
                        Color color = (Color)property.GetValue(null);
                        _knownColors.Add(colorName, color);
                    }
                }
                return _knownColors;
            }
        }

        public static Color FromHex(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            Color? color = TryFromHex(hex);
            if (color.HasValue)
            {
                return color.Value;
            }
            else
            {
                throw new ArgumentException("hex string format error", nameof(hex));
            }
        }

        public static Color FromName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Color? color = TryFromName(name);
            if (color.HasValue)
            {
                return color.Value;
            }
            else
            {
                throw new ArgumentException("unknown color name", nameof(name));
            }
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

        public static Color Inverse(Color value)
        {
            return Color.FromArgb(value.A, (byte)(255 - value.R), (byte)(255 - value.G), (byte)(255 - value.B));
        }

        public static Color Parse(string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Color? color = TryParse(value);
            if (color.HasValue)
            {
                return color.Value;
            }
            else
            {
                throw new ArgumentException("unknown color format", nameof(value));
            }
        }

        public static Color? TryFromHex(string hex)
        {
            if (hex == null)
            {
                return null;
            }

            if (hex.Length == 4)
            {
                Regex regex = new Regex(@"^#([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])$");
                Match match = regex.Match(hex);
                if (match.Success)
                {
                    GroupCollection groups = match.Groups;
                    byte r = byte.Parse(groups[1].Value + groups[1].Value, NumberStyles.HexNumber);
                    byte g = byte.Parse(groups[2].Value + groups[2].Value, NumberStyles.HexNumber);
                    byte b = byte.Parse(groups[3].Value + groups[3].Value, NumberStyles.HexNumber);
                    return FromRgb(r, g, b);
                }
            }
            else if (hex.Length == 7)
            {
                Regex regex = new Regex(@"^#([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$");
                Match match = regex.Match(hex);
                if (match.Success)
                {
                    GroupCollection groups = match.Groups;
                    byte r = byte.Parse(groups[1].Value, NumberStyles.HexNumber);
                    byte g = byte.Parse(groups[2].Value, NumberStyles.HexNumber);
                    byte b = byte.Parse(groups[3].Value, NumberStyles.HexNumber);
                    return FromRgb(r, g, b);
                }
            }

            return null;
        }

        public static Color? TryFromName(string name)
        {
            foreach (var knownColor in KnownColors)
            {
                if (string.Equals(knownColor.Key, name, StringComparison.OrdinalIgnoreCase))
                {
                    return knownColor.Value;
                }
            }
            return null;
        }

        public static Color? TryParse(string value)
        {
            Color? color;
            color = TryFromHex(value);
            if (color.HasValue == false)
            {
                color = TryFromName(value);
            }
            return color;
        }

        private static void InitAccentColorChanged()
        {
            if (_hadInitAccentColorChanged == false)
            {
                ContentControl control = (ContentControl)XamlReader.Load("<ContentControl xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Background =\"{ThemeResource SystemControlBackgroundAccentBrush}\" Visibility=\"Collapsed\" />");
                SolidColorBrush accentColorBrush = (SolidColorBrush)control.Background;
                accentColorBrush.RegisterPropertyChangedCallback(SolidColorBrush.ColorProperty, (sender, dp) =>
                {
                    if (_accentColorChanged != null)
                    {
                        Color accentColor = (Color)sender.GetValue(dp);
                        _accentColorChanged(sender, accentColor);
                    }
                });

                Popup popup = new Popup()
                {
                    Child = control,
                    IsOpen = true
                };

                _hadInitAccentColorChanged = true;
            }
        }
    }
}