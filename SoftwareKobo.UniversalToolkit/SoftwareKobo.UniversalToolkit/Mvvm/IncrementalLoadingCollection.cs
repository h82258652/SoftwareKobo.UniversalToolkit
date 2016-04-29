using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public class IncrementalLoadingCollection<TItem, TItemSource> : ObservableCollection<TItem>, ISupportIncrementalLoading where TItemSource : IncrementalItemSourceBase<TItem>
    {
        private readonly TItemSource _itemSource;

        private bool _hasMoreItems = true;

        private bool _isLoading;

        private DateTime _lastLoadedTime;

        public IncrementalLoadingCollection(TItemSource itemSource)
        {
            if (itemSource == null)
            {
                throw new ArgumentNullException(nameof(itemSource));
            }

            itemSource.HasMoreItemsChanged += (object sender, bool hasMoreItems) =>
            {
                HasMoreItems = hasMoreItems;
            };
            _itemSource = itemSource;
        }

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<uint> LoadMoreCompleted;

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<uint> LoadMoreStarted;

        public bool HasMoreItems
        {
            get
            {
                return _hasMoreItems;
            }
            protected set
            {
                _hasMoreItems = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMoreItems)));
            }
        }

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            protected set
            {
                _isLoading = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        public DateTime LastLoadedTime
        {
            get
            {
                return _lastLoadedTime;
            }
            protected set
            {
                _lastLoadedTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(LastLoadedTime)));
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (IsLoading)
            {
                return Task.FromResult<LoadMoreItemsResult>(new LoadMoreItemsResult()
                {
                    Count = 0
                }).AsAsyncOperation();
            }

            IsLoading = true;
            if (LoadMoreStarted != null)
            {
                LoadMoreStarted(this, count);
            }
            return AsyncInfo.Run(async c =>
            {
                uint resultCount = 0;
                try
                {
                    var beforeLoadCount = Count;
                    await _itemSource.LoadMoreItemsAsync(this, count);
                    var afterLoadCount = Count;

                    if (afterLoadCount > beforeLoadCount)
                    {
                        resultCount = (uint)(afterLoadCount - beforeLoadCount);
                    }

                    LastLoadedTime = DateTime.Now;
                    return new LoadMoreItemsResult()
                    {
                        Count = resultCount
                    };
                }
                catch
                {
                    return new LoadMoreItemsResult()
                    {
                        Count = resultCount
                    };
                }
                finally
                {
                    IsLoading = false;
                    if (LoadMoreCompleted != null)
                    {
                        LoadMoreCompleted(this, resultCount);
                    }
                }
            });
        }

        public async void Refresh()
        {
            ClearItems();
            _itemSource.InternalRefresh(this);

            // 尝试加载 1 项以重置 UI。
            await LoadMoreItemsAsync(1);
        }
    }
}