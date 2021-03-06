﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// Color 扩展类。
    /// </summary>
    public static class ColorExtensions
    {
        private static readonly DependencyProperty AccentColorBrushProperty = DependencyProperty.RegisterAttached("AccentColorBrush", typeof(SolidColorBrush), typeof(ColorExtensions), new PropertyMetadata(null));

        private static IDictionary<string, Color> _knownColors;

        private static UIElement _currentListenElement;

        private static long? _listenToken;

        internal static void ReInitAccentColorChanged()
        {
            if (_currentListenElement != null && _listenToken.HasValue)
            {
                _currentListenElement.UnregisterPropertyChangedCallback(SolidColorBrush.ColorProperty, _listenToken.Value);
            }
            if (_currentListenElement == null)
            {
                _currentListenElement = Window.Current.Content;
                if (_currentListenElement != null)
                {
                    var accentColorBrush = (SolidColorBrush)XamlReader.Load("<SolidColorBrush xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Color=\"{ThemeResource SystemAccentColor}\" />");
                    _listenToken = accentColorBrush.RegisterPropertyChangedCallback(SolidColorBrush.ColorProperty, (obj, dp) =>
                    {
                        if (AccentColorChanged != null)
                        {
                            var accentColor = (Color)obj.GetValue(dp);
                            AccentColorChanged(obj, accentColor);
                        }
                    });
                    _currentListenElement.SetValue(AccentColorBrushProperty, accentColorBrush);
                }
            }
        }

        /// <summary>
        /// 用户主题色发生了变化。
        /// </summary>
        /// <remarks>
        /// 使用此事件后，请勿修改 Window.Current.Content 属性。
        /// 特别感谢 @韦恩卑鄙 的帮助。
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1009")]
        public static event EventHandler<Color> AccentColorChanged;

        /// <summary>
        /// 获取用户主题色。
        /// </summary>
        public static Color AccentColor => (Color)Application.Current.Resources["SystemAccentColor"];

        /// <summary>
        /// 获取 <see cref="Colors"/> 中已经声明的 <see cref="Color"/>。
        /// </summary>
        [SuppressMessage("Microsoft.Compiler", "CS0419")]
        public static IDictionary<string, Color> KnownColors
        {
            get
            {
                if (_knownColors == null)
                {
                    _knownColors = new Dictionary<string, Color>();
                    foreach (var property in typeof(Colors).GetRuntimeProperties())
                    {
                        var colorName = property.Name;
                        var color = (Color)property.GetValue(null);
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

            var color = TryFromHex(hex);
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

            var color = TryFromName(name);
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

        public static Color? TryFromHex(string hex)
        {
            if (hex == null)
            {
                return null;
            }

            if (hex.Length == 4)
            {
                var regex = new Regex(@"^#([0-9A-Fa-f])([0-9A-Fa-f])([0-9A-Fa-f])$");
                var match = regex.Match(hex);
                if (match.Success)
                {
                    var groups = match.Groups;
                    var r = byte.Parse(groups[1].Value + groups[1].Value, NumberStyles.HexNumber);
                    var g = byte.Parse(groups[2].Value + groups[2].Value, NumberStyles.HexNumber);
                    var b = byte.Parse(groups[3].Value + groups[3].Value, NumberStyles.HexNumber);
                    return FromRgb(r, g, b);
                }
            }
            else if (hex.Length == 7)
            {
                var regex = new Regex(@"^#([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})([0-9A-Fa-f]{2})$");
                var match = regex.Match(hex);
                if (match.Success)
                {
                    var groups = match.Groups;
                    var r = byte.Parse(groups[1].Value, NumberStyles.HexNumber);
                    var g = byte.Parse(groups[2].Value, NumberStyles.HexNumber);
                    var b = byte.Parse(groups[3].Value, NumberStyles.HexNumber);
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

        public static Color Parse(string value)
        {
            return (Color)XamlBindingHelper.ConvertValue(typeof(Color), value);
        }

        public static Color? TryParse(string value)
        {
            try
            {
                return Parse(value);
            }
            catch
            {
                return null;
            }
        }
    }
}