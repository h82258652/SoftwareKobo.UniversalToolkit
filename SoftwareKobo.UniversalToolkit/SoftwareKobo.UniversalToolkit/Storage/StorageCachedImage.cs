using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using Windows.ApplicationModel;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UniversalToolkit.Storage
{
    /// <summary>
    /// 带缓存的图片源。
    /// </summary>
    /// <example>
    /// 在 XAML 中使用。
    /// <code>
    /// &lt;Image xmlns:cache=&quot;SoftwareKobo.UniversalToolkit.Storage&quot;&gt;
    ///   &lt;Image.Source&gt;
    ///     &lt;!-- 支持协议：http、https、ms-appx --&gt;
    ///     &lt;cache:StorageCacheImage UriSource=&quot;你的图片路径&quot;/&gt;
    ///   &lt;/Image.Source&gt;
    /// &lt;/Image&gt;
    /// </code>
    /// 在 cs 中使用。
    /// <code>
    /// Image img = new Image();
    /// img.Source = new StorageCachedImage(uri);
    /// </code>
    /// </example>
    public sealed class StorageCachedImage : BitmapSource
    {
        public static readonly DependencyProperty IsAutoRetryProperty = DependencyProperty.Register(nameof(IsAutoRetry), typeof(bool), typeof(StorageCachedImage), new PropertyMetadata(false));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(StorageCachedImage), new PropertyMetadata(false));

        public static readonly DependencyProperty UriSourceProperty = DependencyProperty.Register(nameof(UriSource), typeof(ImageSource), typeof(StorageCachedImage), new PropertyMetadata(null, UriSourceChanged));

        private const string CACHED_IMAGE_DIRECTORY = @"CachedImages";

        public StorageCachedImage()
        {
        }

        public StorageCachedImage(Uri uri)
        {
            this.UriSource = new BitmapImage(uri);
        }

        /// <summary>
        /// 图片加载失败时触发该事件。
        /// </summary>
        public event EventHandler<ImageLoadFailedEventArgs> ImageFailed;

        public bool IsAutoRetry
        {
            get
            {
                return (bool)this.GetValue(IsAutoRetryProperty);
            }
            set
            {
                this.SetValue(IsAutoRetryProperty, value);
            }
        }

        public bool IsLoading
        {
            get
            {
                return (bool)this.GetValue(IsLoadingProperty);
            }
            private set
            {
                this.SetValue(IsLoadingProperty, value);
            }
        }

        public ImageSource UriSource
        {
            get
            {
                return (ImageSource)this.GetValue(UriSourceProperty);
            }
            set
            {
                this.SetValue(UriSourceProperty, value);
            }
        }

        /// <summary>
        /// 清空独立存储中已经缓存的图片。
        /// </summary>
        public static void CleanUpCachedImages()
        {
            IsolatedStorageFileExtensions.DeleteDirectoryRecursive(CACHED_IMAGE_DIRECTORY);
        }

        /// <summary>
        /// 获取已缓存的文件的总大小。
        /// </summary>
        /// <returns>返回图片缓存文件夹的大小，单位为字节。</returns>
        public static long GetCachedImagesSize()
        {
            return IsolatedStorageFileExtensions.GetDirectorySize(CACHED_IMAGE_DIRECTORY);
        }

        private static string GetFilePath(Uri uri)
        {
            return Path.Combine(CACHED_IMAGE_DIRECTORY, WebUtility.UrlEncode(uri.OriginalString));
        }

        private void LoadImage()
        {
        }

        private static async void UriSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            if (e.NewValue == e.OldValue)
            {
                return;
            }

            StorageCachedImage obj = (StorageCachedImage)d;            
            while (true)
            {
                obj.IsLoading = true;
                try
                {
                    #region 清空之前的图像

                    using (var streamSource = new InMemoryRandomAccessStream())
                    {
                        obj.SetSource(streamSource);
                    }

                    #endregion 清空之前的图像

                    BitmapImage value = e.NewValue as BitmapImage;
                    if (value == null)
                    {
                        return;
                    }
                    Uri uri = value.UriSource;
                    if (uri != null && string.IsNullOrEmpty(uri.OriginalString) == false)
                    {
                        string scheme = uri.Scheme;
                        if (scheme == "http" || scheme == "https")
                        {
                            // 网络图像。
                            using (var isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
                            {
                                var filePath = GetFilePath(uri);
                                if (isolatedStorage.FileExists(filePath))
                                {
                                    // 使用本地缓存。
                                    using (var cachedImageStream = isolatedStorage.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                                    {
                                        await obj.SetSourceAsync(cachedImageStream.AsRandomAccessStream());
                                    }
                                }
                                else
                                {
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                                    request.AllowReadStreamBuffering = true;
                                    try
                                    {
                                        using (WebResponse response = await request.GetResponseAsync())
                                        {
                                            using (var stream = response.GetResponseStream())
                                            {
                                                if (isolatedStorage.FileExists(filePath) == false)
                                                {
                                                    if (isolatedStorage.DirectoryExists(CACHED_IMAGE_DIRECTORY) == false)
                                                    {
                                                        isolatedStorage.CreateDirectory(CACHED_IMAGE_DIRECTORY);
                                                    }

                                                    using (var cacheImageFile = isolatedStorage.CreateFile(filePath))
                                                    {
                                                        stream.CopyTo(cacheImageFile);
                                                    }
                                                }

                                                stream.Seek(0, SeekOrigin.Begin);
                                                await obj.SetSourceAsync(stream.AsRandomAccessStream());
                                            }
                                        }
                                    }
                                    catch (WebException)
                                    {
                                        throw;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // 本地图像。
                            var streamSourceReference = RandomAccessStreamReference.CreateFromUri(uri);
                            using (var streamSource = await streamSourceReference.OpenReadAsync())
                            {
                                obj.SetSource(streamSource);
                            }
                        }
                    }
                    // 设置新图像成功。
                    break;
                }
                catch (Exception ex)
                {
                    if (obj.ImageFailed != null)
                    {
                        obj.ImageFailed(obj, new ImageLoadFailedEventArgs(ex));
                    }
                    if (obj.IsAutoRetry)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                finally
                {
                    obj.IsLoading = false;
                }
            }
        }
    }
}