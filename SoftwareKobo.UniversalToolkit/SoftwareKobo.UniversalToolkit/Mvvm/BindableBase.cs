using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可绑定模型基类。
    /// </summary>
    public abstract class BindableBase : INotifyPropertyChanged
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
            RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        /// 通知属性发生变化。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (object.ReferenceEquals(oldValue, newValue))
            {
                return false;
            }
            oldValue = newValue;
            this.RaisePropertyChanged(propertyName);
            return true;
        }
    }
}