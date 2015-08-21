using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可验证的 MVVM 模式模型基类。
    /// </summary>
    public abstract class VerifiableBase : BindableBase
    {
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
        /// 通知属性发生变化。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            base.RaisePropertyChanged(nameof(IsValid));
            base.RaisePropertyChanged(nameof(ValidationResults));
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

        /// <summary>
        /// 获取指定属性的验证结果。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        /// <returns>该属性的验证结果。</returns>
        public virtual ICollection<ValidationResult> GetPropertyValidationResults(string propertyName)
        {
            ValidationContext context = new ValidationContext(this);
            context.MemberName = propertyName;
            var propertyValidationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, propertyValidationResults, false);
            return propertyValidationResults;
        }
    }
}