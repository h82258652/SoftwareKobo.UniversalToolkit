using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
                handler(this, new PropertyChangedEventArgs(nameof(IsValid)));
            }
        }

        /// <summary>
        /// 指示该模型的所有属性是否验证成功。
        /// </summary>
        public virtual bool IsValid
        {
            get
            {
                return ValidationResults.Count <= 0;
            }
        }

        /// <summary>
        /// 获取该模型的验证错误。
        /// </summary>
        public virtual ICollection<ValidationResult> ValidationResults
        {
            get
            {
                ValidationContext context = new ValidationContext(this);
                var validationResults = new List<ValidationResult>();
                Validator.TryValidateObject(this, context, validationResults, true);
                return validationResults;
            }
        }
    }
}