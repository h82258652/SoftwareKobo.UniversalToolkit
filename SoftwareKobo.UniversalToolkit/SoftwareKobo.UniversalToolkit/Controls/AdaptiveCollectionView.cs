using SoftwareKobo.UniversalToolkit.AwaitableUI;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    public class AdaptiveCollectionView : GridView
    {
        public new static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register(nameof(ItemContainerStyle), typeof(Style), typeof(AdaptiveCollectionView), new PropertyMetadata(null, ItemContainerStyleChanged));

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(AdaptiveCollectionViewMode), typeof(AdaptiveCollectionView), new PropertyMetadata(AdaptiveCollectionViewMode.List, InternalModeChanged));

        private new static readonly DependencyProperty ItemsPanelProperty = DependencyProperty.Register(nameof(ItemsPanel), typeof(ItemsPanelTemplate), typeof(AdaptiveCollectionView), new PropertyMetadata(null, ItemsPanelChanged));

        private ItemsPanelTemplate _itemsStackPanel = (ItemsPanelTemplate)XamlReader.Load("<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><ItemsStackPanel Orientation=\"Vertical\" HorizontalAlignment=\"Stretch\" /></ItemsPanelTemplate>");

        private ItemsPanelTemplate _itemsWrapGrid = (ItemsPanelTemplate)XamlReader.Load("<ItemsPanelTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><ItemsWrapGrid Orientation=\"Horizontal\" HorizontalAlignment=\"Center\" /></ItemsPanelTemplate>");

        public AdaptiveCollectionView()
        {
            this.Mode = AdaptiveCollectionViewMode.List;
        }

        public event EventHandler<AdaptiveCollectionViewModeChangedEventArgs> ModeChanged;

        public new Style ItemContainerStyle
        {
            get
            {
                return (Style)this.GetValue(ItemContainerStyleProperty);
            }
            set
            {
                this.SetValue(ItemContainerStyleProperty, value);
            }
        }

        public AdaptiveCollectionViewMode Mode
        {
            get
            {
                return (AdaptiveCollectionViewMode)this.GetValue(ModeProperty);
            }
            set
            {
                this.SetValue(ModeProperty, value);
            }
        }

        private new ItemsPanelTemplate ItemsPanel
        {
            get
            {
                return (ItemsPanelTemplate)this.GetValue(ItemsPanelProperty);
            }
            set
            {
                this.SetValue(ItemsPanelProperty, value);
            }
        }

        private static async void InternalModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdaptiveCollectionView obj = (AdaptiveCollectionView)d;
            AdaptiveCollectionViewMode value = (AdaptiveCollectionViewMode)e.NewValue;

            if (Enum.IsDefined(typeof(AdaptiveCollectionViewMode), value) == false)
            {
                throw new ArgumentException($"{nameof(AdaptiveCollectionViewMode)} is not defined", nameof(value));
            }

            int index = -1;
            if (obj.Items.Count > 0)
            {
                ItemsStackPanel stackPanel = obj.ItemsPanelRoot as ItemsStackPanel;
                if (stackPanel != null)
                {
                    index = stackPanel.FirstVisibleIndex;
                }

                ItemsWrapGrid wrapGrid = obj.ItemsPanelRoot as ItemsWrapGrid;
                if (wrapGrid != null)
                {
                    index = wrapGrid.FirstVisibleIndex;
                }
            }

            if (value == AdaptiveCollectionViewMode.List)
            {
                obj.ItemsPanel = obj._itemsStackPanel;
            }
            else
            {
                obj.ItemsPanel = obj._itemsWrapGrid;
            }

            if (obj.ModeChanged != null)
            {
                obj.ModeChanged(obj, new AdaptiveCollectionViewModeChangedEventArgs((AdaptiveCollectionViewMode)e.OldValue, (AdaptiveCollectionViewMode)e.NewValue));
            }

            await obj.WaitForLayoutUpdatedAsync();

            if (obj.SelectedItem != null)
            {
                obj.ScrollIntoView(obj.SelectedItem, ScrollIntoViewAlignment.Leading);
            }
            else if (index > -1)
            {
                var item = obj.Items.ElementAtOrDefault(index);
                if (item != null)
                {
                    obj.ScrollIntoView(item, ScrollIntoViewAlignment.Leading);
                }
            }
        }

        private static void ItemContainerStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdaptiveCollectionView obj = (AdaptiveCollectionView)d;
            Style value = (Style)e.NewValue;

            if (value != null)
            {
                if (value.TargetType != typeof(AdaptiveCollectionViewItem))
                {
                    throw new ArgumentException($"the target type of item container style should be {nameof(AdaptiveCollectionViewItem)}", nameof(value));
                }
                else
                {
                    value.TargetType = typeof(GridViewItem);
                }
            }

            obj.SetBaseItemContainerStyle(value);
        }

        private static void ItemsPanelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdaptiveCollectionView obj = (AdaptiveCollectionView)d;
            ItemsPanelTemplate value = (ItemsPanelTemplate)e.NewValue;

            obj.SetBaseItemsPanel(value);
        }

        private void SetBaseItemContainerStyle(Style value)
        {
            base.ItemContainerStyle = value;
        }

        private void SetBaseItemsPanel(ItemsPanelTemplate value)
        {
            base.ItemsPanel = value;
        }
    }
}