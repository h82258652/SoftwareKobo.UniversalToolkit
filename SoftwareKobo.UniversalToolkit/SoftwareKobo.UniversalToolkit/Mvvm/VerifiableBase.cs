using System.Runtime.CompilerServices;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可验证的 MVVM 模式模型基类。
    /// </summary>
    /// <example>
    /// &lt;TextBox Text="{Binding Path=Person.Name,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged}" /&gt;
    /// 某个属性 Errors（只显示该属性错误的第一条消息）：
    /// &lt;TextBlock Text="{Binding Path=Person.Errors[Name]}" /&gt;
    /// 所有属性 Errors（以换行符分隔每一条错误消息）：
    /// &lt;TextBlock Text="{Binding Path=Person.Errors}" /&gt;
    /// </example>
    public abstract class VerifiableBase : BindableBase
    {
        public ModelVerifyErrors Errors => new ModelVerifyErrors(this);

        /// <summary>
        /// 指示该模型的所有属性是否验证成功。
        /// </summary>
        public bool IsValid => Errors.Count <= 0;

        /// <summary>
        /// 通知属性发生变化。
        /// </summary>
        /// <param name="propertyName">属性名称。</param>
        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            base.RaisePropertyChanged(nameof(Errors));
            base.RaisePropertyChanged(nameof(IsValid));
        }
    }
}