using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    public class IncrementalLoadingCollection<TItem, TIncrementalItemSource> : ObservableCollection<TItem>, ISupportIncrementalLoading where TIncrementalItemSource : IIncrementalItemSource<TItem>
    {
        private readonly TIncrementalItemSource _incrementalItemSource;

        private bool _hasMoreItems = true;

        private bool _isLoading;

        private DateTime _lastLoadedTime;
        
        public IncrementalLoadingCollection(TIncrementalItemSource incrementalItemSource)
        {
            if (incrementalItemSource == null)
            {
                throw new ArgumentNullException(nameof(incrementalItemSource));
            }

            incrementalItemSource.RaiseHasMoreItemsChanged += (sender, hasMoreItems) =>
            {
                this.HasMoreItems = hasMoreItems;
            };
            this._incrementalItemSource = incrementalItemSource;
        }

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
            set
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
            return AsyncInfo.Run(async c =>
            {
                try
                {
                    uint resultCount = await this._incrementalItemSource.LoadMoreItemsAsync(this, count);
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
                        Count = 0
                    };
                }
                finally
                {
                    this.IsLoading = false;
                }
            });
        }
    }
}