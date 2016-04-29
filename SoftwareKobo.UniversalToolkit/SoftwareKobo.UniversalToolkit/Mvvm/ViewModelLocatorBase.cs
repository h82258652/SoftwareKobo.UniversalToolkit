namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 视图模型定位器基类。
    /// </summary>
    public abstract class ViewModelLocatorBase
    {
        private readonly IoC _container = new IoC();

        /// <summary>
        /// 指示当前是否处于设计模式。
        /// </summary>
        public static bool IsInDesignMode => ViewModelBase.IsInDesignMode;

        /// <summary>
        /// 获取注入类的实例。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        /// <returns>实例。</returns>
        protected virtual T GetInstance<T>() where T : class
        {
            return _container.GetInstance<T>();
        }

        /// <summary>
        /// 注册注入类。
        /// </summary>
        /// <typeparam name="T">注入类类型。</typeparam>
        protected virtual void Register<T>() where T : class
        {
            _container.Register<T>();
        }

        /// <summary>
        /// 注册注入类。
        /// </summary>
        /// <typeparam name="TFrom">注入类基类或实现的接口。</typeparam>
        /// <typeparam name="TTo">注入类类型。</typeparam>
        protected virtual void Register<TFrom, TTo>() where TTo : class, TFrom where TFrom : class
        {
            _container.Register<TFrom, TTo>();
        }
    }
}