using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

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
        public static readonly DependencyProperty IsAutoRetryProperty = DependencyProperty.Register(nameof(IsAutoRetry), typeof(bool), typeof(StorageCachedImage), new PropertyMetadata(false, IsAutoRetryChanged));

        public static readonly DependencyProperty IsLoadingProperty = DependencyProperty.Register(nameof(IsLoading), typeof(bool), typeof(StorageCachedImage), new PropertyMetadata(false));

        public static readonly DependencyProperty UriSourceProperty = DependencyProperty.Register(nameof(UriSource), typeof(ImageSource), typeof(StorageCachedImage), new PropertyMetadata(null, UriSourceChanged));

        private const string CACHED_IMAGE_DIRECTORY = @"CachedImages";

        private EventHandler<ImageFailedEventArgs> _autoRetryHandler;

        private DateTime _lastRequestTime;

        public StorageCachedImage()
        {
            _autoRetryHandler = (sender, e) =>
            {
                UriSourceChanged();
            };
        }

        public StorageCachedImage(Uri uri) : this()
        {
            this.UriSource = new BitmapImage(uri);
        }

        public event EventHandler<ImageFailedEventArgs> ImageFailed;

        public event RoutedEventHandler ImageOpened;

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

        public static bool CachedImageExist(Uri uri)
        {
            string cachePath = GetCachePath(uri);
            return IsCacheExist(cachePath);
        }

        public static void RemoveCachedImage(Uri uri)
        {
            string cachePath = GetCachePath(uri);
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                isf.DeleteFile(cachePath);
            }
        }

        private static string GetCachePath(Uri uri)
        {
            return Path.Combine(CACHED_IMAGE_DIRECTORY, WebUtility.UrlEncode(uri.OriginalString));
        }

        private static void IsAutoRetryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            StorageCachedImage obj = (StorageCachedImage)d;
            bool value = (bool)e.NewValue;
            if (value)
            {
                obj.ImageFailed += obj._autoRetryHandler;
            }
            else
            {
                obj.ImageFailed -= obj._autoRetryHandler;
            }
        }

        private static bool IsCacheExist(string cachedPath)
        {
            if (DesignMode.DesignModeEnabled)
            {
                // 设计模式下，独立存储不可用，直接返回不存在。
                return false;
            }

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return isf.FileExists(cachedPath);
            }
        }

        private static async Task<byte[]> LoadCachedImageAsync(string cachePath)
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fs = isf.OpenFile(cachePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        await fs.CopyToAsync(stream);
                        return stream.ToArray();
                    }
                }
            }
        }

        private static async Task SaveImageAsync(string cachePath, byte[] datas)
        {
            if (DesignMode.DesignModeEnabled)
            {
                // 设计模式下，独立存储不可用。
                return;
            }

            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(cachePath))
                {
                    return;
                }

                if (isf.DirectoryExists(CACHED_IMAGE_DIRECTORY) == false)
                {
                    isf.CreateDirectory(CACHED_IMAGE_DIRECTORY);
                }

                using (var fs = isf.CreateFile(cachePath))
                {
                    await fs.WriteAsync(datas, 0, datas.Length);
                }
            }
        }

        private static void UriSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == e.OldValue)
            {
                return;
            }

            StorageCachedImage obj = (StorageCachedImage)d;
            obj.UriSourceChanged();
        }

        private async Task SetStreamAsync(IRandomAccessStream stream, DateTime requestTime)
        {
            if (_lastRequestTime == requestTime)
            {
                // 确保为最后设置的 Uri 对应的图像。
                await this.SetSourceAsync(stream);
            }
        }

        public void SetUriSource(Uri uriSource)
        {
            this.UriSource = new BitmapImage(uriSource);
        }

        private async void UriSourceChanged()
        {
            this.IsLoading = true;

            #region 清空之前的图像

            using (var emptyStream = new InMemoryRandomAccessStream())
            {
                this.SetSource(emptyStream);
            }

            #endregion 清空之前的图像

            // 清空图像。
            if (UriSource == null)
            {
                this.IsLoading = false;
                return;
            }

            BitmapImage bitmapImage = UriSource as BitmapImage;
            Uri uri = bitmapImage != null ? bitmapImage.UriSource : null;
            if (uri == null)
            {
                this.IsLoading = false;
                throw new InvalidOperationException("not support image source");
            }

            var requestTime = DateTime.Now;
            this._lastRequestTime = requestTime;

            string scheme = uri.Scheme;
            if (scheme == "http" || scheme == "https")
            {
                // 网络图像。
                byte[] networkImageDatas;

                // 获取本地对应的缓存路径。
                string cachePath = GetCachePath(uri);

                // 本地图片缓存文件是否存在。
                bool isCacheExist = IsCacheExist(cachePath);

                if (isCacheExist == false)
                {
                    // 缓存不存在，下载图像。
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            networkImageDatas = (await client.GetBufferAsync(uri)).ToArray();
                        }
                        catch (WebException ex)
                        {
                            // 下载失败。
                            if (this.ImageFailed != null)
                            {
                                this.ImageFailed(this, new ImageFailedEventArgs(ex));
                            }
                            this.IsLoading = false;
                            return;
                        }
                    }

                    // 下载成功，保存到独立存储。
                    await SaveImageAsync(cachePath, networkImageDatas);
                }
                else
                {
                    // 缓存存在，加载独立存储中的缓存图像。
                    networkImageDatas = await LoadCachedImageAsync(cachePath);
                }

                // 加载图像。
                using (MemoryStream stream = new MemoryStream(networkImageDatas))
                {
                    try
                    {
                        await this.SetStreamAsync(stream.AsRandomAccessStream(), requestTime);
                    }
                    catch (Exception ex)
                    {
                        if (this.ImageFailed != null)
                        {
                            this.ImageFailed(this, new ImageFailedEventArgs(ex));
                        }

                        // 丢弃缓存。
                        RemoveCachedImage(uri);

                        this.IsLoading = false;
                        return;
                    }
                }
            }
            else
            {
                // 本地图像。
                if (DesignMode.DesignModeEnabled == false)
                {
                    RandomAccessStreamReference reference = RandomAccessStreamReference.CreateFromUri(uri);
                    using (var stream = await reference.OpenReadAsync())
                    {
                        try
                        {
                            await this.SetStreamAsync(stream, requestTime);
                        }
                        catch (Exception ex)
                        {
                            if (this.ImageFailed != null)
                            {
                                this.ImageFailed(this, new ImageFailedEventArgs(ex));
                            }
                            this.IsLoading = false;
                            return;
                        }
                    }
                }
            }

            // 加载成功。
            if (this.ImageOpened != null)
            {
                this.ImageOpened(this, new RoutedEventArgs());
            }
            this.IsLoading = false;
        }
    }
}