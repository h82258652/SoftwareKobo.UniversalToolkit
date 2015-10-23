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
                this.HasMoreItems = hasMoreItems;
            };
            this._itemSource = itemSource;
        }

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<uint> LoadMoreCompleted;

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<uint> LoadMoreStarted;

        public bool HasMoreItems
        {
            get
            {
                return this._hasMoreItems;
            }
            protected set
            {
                this._hasMoreItems = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(HasMoreItems)));
            }
        }

        public bool IsLoading
        {
            get
            {
                return this._isLoading;
            }
            protected set
            {
                this._isLoading = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsLoading)));
            }
        }

        public DateTime LastLoadedTime
        {
            get
            {
                return this._lastLoadedTime;
            }
            protected set
            {
                this._lastLoadedTime = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(LastLoadedTime)));
            }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (this.IsLoading)
            {
                return Task.FromResult<LoadMoreItemsResult>(new LoadMoreItemsResult()
                {
                    Count = 0
                }).AsAsyncOperation();
            }

            this.IsLoading = true;
            if (this.LoadMoreStarted != null)
            {
                this.LoadMoreStarted(this, count);
            }
            return AsyncInfo.Run(async c =>
            {
                uint resultCount = 0;
                try
                {
                    int beforeLoadCount = this.Count;
                    await this._itemSource.LoadMoreItemsAsync(this, count);
                    int afterLoadCount = this.Count;

                    if (afterLoadCount > beforeLoadCount)
                    {
                        resultCount = (uint)(afterLoadCount - beforeLoadCount);
                    }

                    this.LastLoadedTime = DateTime.Now;
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
                    this.IsLoading = false;
                    if (this.LoadMoreCompleted != null)
                    {
                        this.LoadMoreCompleted(this, resultCount);
                    }
                }
            });
        }

        public async void Refresh()
        {
            this.ClearItems();
            this._itemSource.InternalRefresh(this);

            // 尝试加载 1 项以重置 UI。
            await this.LoadMoreItemsAsync(1);
        }
    }
}