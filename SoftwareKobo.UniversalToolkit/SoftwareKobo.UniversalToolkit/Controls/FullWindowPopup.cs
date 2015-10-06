using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UniversalToolkit.Controls
{
    [ContentProperty(Name = nameof(Child))]
    public sealed class FullWindowPopup : DependencyObject
    {
        public static readonly DependencyProperty AttachedPopupProperty = DependencyProperty.RegisterAttached("AttachedPopup", typeof(FullWindowPopup), typeof(FullWindowPopup), new PropertyMetadata(null));

        public static readonly DependencyProperty ChildProperty = DependencyProperty.Register(nameof(Child), typeof(UIElement), typeof(FullWindowPopup), new PropertyMetadata(null, ChildChanged));

        public static readonly DependencyProperty ChildTransitionsProperty = DependencyProperty.Register(nameof(ChildTransitions), typeof(TransitionCollection), typeof(FullWindowPopup), new PropertyMetadata(null, ChildTransitionsChanged));

        public static readonly DependencyProperty IsLightDismissEnabledProperty = DependencyProperty.Register(nameof(IsLightDismissEnabled), typeof(bool), typeof(FullWindowPopup), new PropertyMetadata(false, IsLightDismissEnabledChanged));

        public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof(IsOpen), typeof(bool), typeof(FullWindowPopup), new PropertyMetadata(false, IsOpenChanged));

        private ContentControl _contentControl;

        private Popup _popup;

        public FullWindowPopup()
        {
            _contentControl = new ContentControl()
            {
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch
            };

            _popup = new Popup()
            {
                Child = _contentControl
            };
            _popup.Closed += delegate
            {
                this.IsOpen = false;
            };

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

        public static FullWindowPopup GetAttachedPopup(FrameworkElement obj)
        {
            return (FullWindowPopup)obj.GetValue(AttachedPopupProperty);
        }

        public static void SetAttachedPopup(FrameworkElement obj, FullWindowPopup value)
        {
            obj.SetValue(AttachedPopupProperty, value);
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