using SoftwareKobo.UniversalToolkit.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可绑定模型基类。
    /// </summary>
    public abstract class BindableBase : DisposableObject, INotifyPropertyChanged
    {
        /// <summary>
        /// 在属性值时被更改时发生。
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知该对象所有属性发生变化。
        /// </summary>
        protected virtual void RaiseAllPropertiesChanged()
        {
            this.RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// 通知属性发生变化。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            this.VerifyPropertyName(propertyName);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            this.RaisePropertyChanged(ExpressionResolver.ResolvePropertyName(propertyExpression));
        }

        protected bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
            {
                return false;
            }
            oldValue = newValue;
            this.RaisePropertyChanged(propertyName);
            return true;
        }

        protected bool Set<T>(Expression<Func<T>> propertyExpression, ref T oldValue, T newValue)
        {
            return this.Set(ref oldValue, newValue, ExpressionResolver.ResolvePropertyName(propertyExpression));
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

            var propertiesNames = this.GetType().GetRuntimeProperties().Select(temp => temp.Name);

            if (propertiesNames.Contains(propertyName) == false)
            {
                throw new ArgumentException("property is not exist.", nameof(propertyName));
            }
        }
    }
}