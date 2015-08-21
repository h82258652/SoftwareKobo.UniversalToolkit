using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型定位器基类。
    /// </summary>
    public abstract class ViewModelLocatorBase
    {
        private static UnityContainer _defaultContainer;
        private static UnityServiceLocator _defaultLocator;

        /// <summary>
        /// 使用 Unity 容器作为默认容器并初始化视图模型定位器。
        /// </summary>
        protected ViewModelLocatorBase()
        {
            if (ServiceLocator.IsLocationProviderSet == false && _defaultContainer == null && _defaultLocator == null)
            {
                _defaultContainer = new UnityContainer();
                _defaultLocator = new UnityServiceLocator(_defaultContainer);
                ServiceLocator.SetLocatorProvider(() => _defaultLocator);
            }
        }

        /// <summary>
        /// 使用自定义服务定位器来初始化视图模型定位器。
        /// </summary>
        /// <param name="provider">获取服务定位器的方法。</param>
        /// <remarks>使用该构造函数时，请重写两个注册方法。</remarks>
        protected ViewModelLocatorBase(ServiceLocatorProvider provider)
        {
            ServiceLocator.SetLocatorProvider(provider);
        }

        /// <summary>
        /// 注册注入类到默认容器。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        protected virtual void Register<T>() where T : class
        {
            _defaultContainer.RegisterType<T>();
        }

        /// <summary>
        /// 注册注入类到默认容器。。
        /// </summary>
        /// <typeparam name="TFrom">注入类基类或实现的接口。</typeparam>
        /// <typeparam name="TTo">注入类类型。</typeparam>
        protected virtual void Register<TFrom, TTo>() where TTo : TFrom
        {
            _defaultContainer.RegisterType<TFrom, TTo>();
        }

        /// <summary>
        /// 获取注入类的实例。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        /// <returns>实例。</returns>
        protected virtual T GetInstance<T>()
        {
            return ServiceLocator.Current.GetInstance<T>();
        }
    }
}