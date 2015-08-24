using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System;
using Windows.Web.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using System.IO.IsolatedStorage;
using System.Net;
using System.IO;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    public static class ImageCache
    {
        public static void SetIsCacheEnabled(Image obj, bool value)
        {
            obj.SetValue(IsCacheEnabledProperty, value);
        }

        public static bool GetIsCacheEnabled(Image obj)
        {
            return (bool)obj.GetValue(IsCacheEnabledProperty);
        }

        public static readonly DependencyProperty IsCacheEnabledProperty =
            DependencyProperty.Register("IsCacheEnabled", typeof(bool), typeof(ImageCache), new PropertyMetadata(false, IsCacheEnabledChanged));

        private static void IsCacheEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }
            Image obj = (Image)d;
            bool value = (bool)e.NewValue;
            if (value)
            {
                long id = obj.RegisterPropertyChangedCallback(Image.SourceProperty, async (sender, dp) =>
                 {
                     BitmapImage source = sender.GetValue(dp) as BitmapImage;
                     if (source != null)
                     {
                         var uri = source.UriSource;
                         string scheme = uri.Scheme;
                         if (scheme == "http" || scheme == "https")
                         {
                             var fileName = GetFileNameInIsolatedStorage(uri);
                             if (_isolatedStorage.FileExists(fileName))
                             {
                                 using (var fileStream= _isolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                                 {
                                     BitmapImage sourceCache = new BitmapImage();
                                     sourceCache.SetSource(fileStream.AsRandomAccessStream());
                                     sender.SetValue(dp, sourceCache);
                                 }
                             }
                             else
                             {
                                 HttpClient client = new HttpClient();
                               var buffer= await   client.GetBufferAsync
                                (uri);
                                var sourceCache= buffer.AsStream();

                                 if (_isolatedStorage.FileExists(fileName)== false)
                                 {
                                     var file= _isolatedStorage.CreateFile
                                     (fileName);
                                     sourceCache.CopyTo(file);
                                 }
                                 


                                 // load from web and storage the file.
                                 // TODO
#warning not finish
                             }
                         }
                     }
                 });
                _registeredCallback.Add(obj, id);
            }
            else
            {
                long id = _registeredCallback[obj];
                obj.UnregisterPropertyChangedCallback(Image.SourceProperty, id);
            }
        }

        private static IsolatedStorageFile _isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();

        private const string IMAGE_CACHE_FOLDER = @"ImageCaches";

        private static string GetFileNameInIsolatedStorage(Uri imageUri)
        {
            return Path.Combine(IMAGE_CACHE_FOLDER, WebUtility.UrlEncode(imageUri.OriginalString));
        }

        private static Dictionary<Image, long> _registeredCallback = new Dictionary<Image, long>();
    }
}