using Microsoft.Practices.Unity;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型定位器基类。
    /// </summary>
    public abstract class ViewModelLocatorBase : UnityContainer
    {
        /// <summary>
        /// 注册注入类。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        protected virtual void Register<T>() where T : class
        {
            this.RegisterType<T>();
        }

        /// <summary>
        /// 注册注入类。
        /// </summary>
        /// <typeparam name="TFrom">注入类基类或实现的接口。</typeparam>
        /// <typeparam name="TTo">注入类类型。</typeparam>
        protected virtual void Register<TFrom, TTo>() where TTo : TFrom
        {
            this.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// 获取注入类的实例。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        /// <returns>实例。</returns>
        protected virtual T GetInstance<T>()
        {
            return this.Resolve<T>();
        }
    }
}