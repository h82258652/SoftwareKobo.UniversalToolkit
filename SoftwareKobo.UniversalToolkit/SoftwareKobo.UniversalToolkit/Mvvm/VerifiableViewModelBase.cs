using System.Runtime.CompilerServices;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 可验证的视图模型基类。
    /// </summary>
    public abstract class VerifiableViewModelBase : ViewModelBase
    {
        public ModelVerifyErrors Errors => new ModelVerifyErrors(this);

        public bool IsValid => Errors.Count <= 0;

        protected override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.RaisePropertyChanged(propertyName);
            base.RaisePropertyChanged(nameof(Errors));
            base.RaisePropertyChanged(nameof(IsValid));
        }
    }
}