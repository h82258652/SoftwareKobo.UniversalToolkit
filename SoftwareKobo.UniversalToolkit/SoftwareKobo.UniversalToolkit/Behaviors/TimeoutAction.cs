using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace SoftwareKobo.UniversalToolkit.Behaviors
{
    [ContentProperty(Name = nameof(Actions))]
    public sealed class TimeoutAction : DependencyObject, IAction
    {
        public static readonly DependencyProperty ActionsProperty = DependencyProperty.Register(nameof(Actions), typeof(ActionCollection), typeof(TimeoutAction), new PropertyMetadata(null));

        public static readonly DependencyProperty IsConcurrentProperty = DependencyProperty.Register(nameof(IsConcurrent), typeof(bool), typeof(TimeoutAction), new PropertyMetadata(false));

        public static readonly DependencyProperty TimeoutProperty = DependencyProperty.Register(nameof(Timeout), typeof(TimeSpan), typeof(TimeoutAction), new PropertyMetadata(TimeSpan.Zero));

        private EventHandler<object> _lastTimerTickHandler;

        private DispatcherTimer _timer;

        public ActionCollection Actions
        {
            get
            {
                ActionCollection actions = (ActionCollection)this.GetValue(ActionsProperty);
                if (actions == null)
                {
                    actions = new ActionCollection();
                    this.SetValue(ActionsProperty, actions);
                }
                return actions;
            }
        }

        /// <summary>
        /// 设置或获取是否并发执行。若为 false，则上一次执行未开始的话，会取消掉上一次执行。若为 true，则上一次仍然执行。默认为 false。
        /// </summary>
        public bool IsConcurrent
        {
            get
            {
                return (bool)this.GetValue(IsConcurrentProperty);
            }
            set
            {
                this.SetValue(IsConcurrentProperty, value);
            }
        }

        public TimeSpan Timeout
        {
            get
            {
                return (TimeSpan)this.GetValue(TimeoutProperty);
            }
            set
            {
                this.SetValue(TimeoutProperty, value);
            }
        }

        public object Execute(object sender, object parameter)
        {
            DispatcherTimer timer;

            if (this.IsConcurrent == false)
            {
                // 不并发，使用上一个 timer。
                if (this._timer != null)
                {
                    // 取消上一个
                    this._timer.Tick -= this._lastTimerTickHandler;
                    this._timer.Stop();
                }

                this._timer = new DispatcherTimer()
                {
                    Interval = this.Timeout
                };
                timer = this._timer;
            }
            else
            {
                // 并发，使用新 timer。
                timer = new DispatcherTimer()
                {
                    Interval = this.Timeout
                };
            }

            EventHandler<object> tickHandler = null;
            tickHandler = delegate
            {
                timer.Tick -= tickHandler;
                timer.Stop();

                Interaction.ExecuteActions(sender, this.Actions, parameter);
            };
            timer.Tick += tickHandler;

            if (this.IsConcurrent == false)
            {
                this._lastTimerTickHandler = tickHandler;
            }

            timer.Start();

            return null;
        }
    }
}