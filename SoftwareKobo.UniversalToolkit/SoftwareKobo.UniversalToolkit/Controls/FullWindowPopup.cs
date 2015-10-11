using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    /// <summary>
    /// 一个占满当前线程窗口的 Popup。
    /// </summary>
    [ContentProperty(Name = nameof(Child))]
    public sealed class FullWindowPopup : FrameworkElement
    {
        public static readonly DependencyProperty AttachedPopupProperty = DependencyProperty.RegisterAttached("AttachedPopup", typeof(FullWindowPopup), typeof(FullWindowPopup), new PropertyMetadata(null, AttachedPopupChanged));

        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(nameof(Child), typeof(UIElement), typeof(FullWindowPopup), new PropertyMetadata(null, ChildChanged));

        public static readonly DependencyProperty ChildTransitionsProperty = DependencyProperty.Register(nameof(ChildTransitions), typeof(TransitionCollection), typeof(FullWindowPopup), new PropertyMetadata(null, ChildTransitionsChanged));

        public static readonly DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(nameof(IsLightDismissEnabled), typeof(bool), typeof(FullWindowPopup), new PropertyMetadata(false, IsLightDismissEnabledChanged));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(FullWindowPopup), new PropertyMetadata(false, IsOpenChanged));

        private TypedEventHandler<FrameworkElement, DataContextChangedEventArgs> _attachedObjectDataContextChangedHandler;

        private ContentControl _contentControl;

        private Popup _popup;

        public FullWindowPopup()
        {
            _contentControl = new ContentControl()
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

            this._popup = new Popup()
            {
                Child = _contentControl
            };
            Binding binding = new Binding()
            {
                Source = this,
                Path = new PropertyPath(nameof(IsOpen)),
                Mode = BindingMode.TwoWay
            };
            this._popup.SetBinding(Popup.IsOpenProperty, binding);

            Action keepPopupSize = () =>
            {
                Window.Current.SizeChanged += delegate
                {
                    this.ReSize();
                };
                this.ReSize();
            };

            Bootstrapper app = Bootstrapper.Current;
            if (app != null && app.IsInConstructing)
            {
                // 应用程序的 App 类继承自 Bootstrapper，并且当前处于 App 类的构造函数中，将保持 Popup 大小的方法放至构造函数执行完成后再执行。
                app._waitForConstructedActions.Add(() =>
                {
                    keepPopupSize.Invoke();
                    return Task.FromResult<object>(null);
                });
            }
            else
            {
                keepPopupSize.Invoke();
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<object> Closed
        {
            add
            {
                this._popup.Closed += value;
            }
            remove
            {
                this._popup.Closed -= value;
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1009")]
        public event EventHandler<object> Opened
        {
            add
            {
                this._popup.Opened += value;
            }
            remove
            {
                this._popup.Opened -= value;
            }
        }

        public UIElement Child
        {
            get
            {
                return (UIElement)this.GetValue(ChildProperty);
            }
            set
            {
                this.SetValue(ChildProperty, value);
            }
        }

        public TransitionCollection ChildTransitions
        {
            get
            {
                return (TransitionCollection)this.GetValue(ChildTransitionsProperty);
            }
            set
            {
                this.SetValue(ChildTransitionsProperty, value);
            }
        }

        public bool IsLightDismissEnabled
        {
            get
            {
                return (bool)this.GetValue(IsLightDismissEnabledProperty);
            }
            set
            {
                this.SetValue(IsLightDismissEnabledProperty, value);
            }
        }

        public bool IsOpen
        {
            get
            {
                return (bool)this.GetValue(IsOpenProperty);
            }
            set
            {
                this.SetValue(IsOpenProperty, value);
            }
        }

        private TypedEventHandler<FrameworkElement, DataContextChangedEventArgs> AttachedObjectDataContextChangedHandler
        {
            get
            {
                if (_attachedObjectDataContextChangedHandler == null)
                {
                    _attachedObjectDataContextChangedHandler = (sender, e) =>
                    {
                        this.DataContext = e.NewValue;
                        this._popup.DataContext = e.NewValue;
                    };
                }
                return _attachedObjectDataContextChangedHandler;
            }
        }

        public static FullWindowPopup GetAttachedPopup(FrameworkElement obj)
        {
            return (FullWindowPopup)obj.GetValue(AttachedPopupProperty);
        }

        /// <summary>
        /// 将一个 FullWindowPopup 附加到一个 FrameworkElement 上。
        /// </summary>
        /// <param name="obj">被附加的 FrameworkElement。</param>
        /// <param name="value">附加的 FullWindowPopup。</param>
        public static void SetAttachedPopup(FrameworkElement obj, FullWindowPopup value)
        {
            obj.SetValue(AttachedPopupProperty, value);
        }

        private static void AttachedPopupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement obj = d as FrameworkElement;
            if (obj != null)
            {
                FullWindowPopup oldPopup = (FullWindowPopup)e.OldValue;
                if (oldPopup != null)
                {
                    obj.DataContextChanged -= oldPopup.AttachedObjectDataContextChangedHandler;
                }

                FullWindowPopup newPopup = (FullWindowPopup)e.NewValue;
                if (newPopup != null)
                {
                    obj.DataContextChanged += newPopup.AttachedObjectDataContextChangedHandler;
                }
            }
        }

        private static void ChildChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FullWindowPopup obj = (FullWindowPopup)d;
            UIElement value = (UIElement)e.NewValue;
            obj._contentControl.Content = value;
        }

        private static void ChildTransitionsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FullWindowPopup obj = (FullWindowPopup)d;
            TransitionCollection value = (TransitionCollection)e.NewValue;
            obj._popup.ChildTransitions = value;
        }

        private static void IsLightDismissEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FullWindowPopup obj = (FullWindowPopup)d;
            bool value = (bool)e.NewValue;
            obj._popup.IsLightDismissEnabled = value;
        }

        private static void IsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FullWindowPopup obj = (FullWindowPopup)d;
            bool value = (bool)e.NewValue;
            obj._popup.IsOpen = value;
        }

        private void ReSize()
        {
            Rect size = Window.Current.Bounds;
            _contentControl.Width = size.Width;
            _contentControl.Height = size.Height;
        }
    }
}