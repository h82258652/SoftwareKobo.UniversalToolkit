using SoftwareKobo.UniversalToolkit.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Windows.UI.Core;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可绑定模型基类。
    /// </summary>
    public abstract class BindableBase : DisposableObject, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static readonly CoreWindow CoreWindow = CoreWindow.GetForCurrentThread();

        /// <summary>
        /// 在属性值更改后发生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 在属性值更改前发生。
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <summary>
        /// 通知该对象所有属性发生了变化。
        /// </summary>
        protected virtual void RaiseAllPropertiesChanged()
        {
            RaisePropertyChanged(string.Empty);
        }

        protected virtual void RaiseAllPropertiesChanging()
        {
            RaisePropertyChanging(string.Empty);
        }

        /// <summary>
        /// 通知属性发生变化。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        protected virtual async void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            if (PropertyChanged == null)
            {
                return;
            }
            if (CoreWindow != null && CoreWindow.Dispatcher.HasThreadAccess == false)
            {
                await CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                });
            }
            else
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(ExpressionResolver.ResolvePropertyName(propertyExpression));
        }

        protected virtual async void RaisePropertyChanging([CallerMemberName]string propertyName = null)
        {
            VerifyPropertyName(propertyName);
            if (PropertyChanging == null)
            {
                return;
            }
            if (CoreWindow != null && CoreWindow.Dispatcher.HasThreadAccess == false)
            {
                await CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
                });
            }
            else
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanging(ExpressionResolver.ResolvePropertyName(propertyExpression));
        }

        protected bool Set<T>(ref T storage, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, newValue))
            {
                return false;
            }
            RaisePropertyChanging(propertyName);
            storage = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T storage, T newValue)
        {
            return Set(ref storage, newValue, ExpressionResolver.ResolvePropertyName(propertyExpression));
        }

        /// <summary>
        /// 校验属性名称是否存在。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (propertyName.Length <= 0)
            {
                // Raise all properties changed.
                return;
            }

            var propertiesNames = GetType().GetRuntimeProperties().Select(temp => temp.Name);

            if (propertiesNames.Contains(propertyName) == false)
            {
                throw new ArgumentException("property is not exist.", nameof(propertyName));
            }
        }
    }
}