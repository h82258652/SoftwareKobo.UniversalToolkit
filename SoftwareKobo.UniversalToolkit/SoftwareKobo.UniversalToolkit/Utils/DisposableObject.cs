using System;

namespace SoftwareKobo.UniversalToolkit.Utils
{
    /// <summary>
    /// 可释放对象。
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>
        /// 指示该对象是否已经释放。
        /// </summary>
        private bool _hadDisposed;

        ~DisposableObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放托管对象。
        /// </summary>
        /// <param name="disposing">false 时为析构函数调用，true 时为 Dispose 方法调用。</param>
        protected virtual void DisposeManagedObjects(bool disposing)
        {
        }

        /// <summary>
        /// 释放非托管对象。将对象的引用设置为 null 即可。
        /// </summary>
        protected virtual void DisposeUnmanagedObjects()
        {
        }

        /// <summary>
        /// 执行与释放或重置非托管资源关联的应用程序定义的任务。
        /// </summary>
        /// <param name="isDisposingByDispose">true 为由 Dispose 方法调用，false 为析构函数调用。</param>
        private void Dispose(bool isDisposingByDispose)
        {
            if (_hadDisposed == false)
            {
                if (isDisposingByDispose)
                {
                    DisposeManagedObjects(isDisposingByDispose);
                }

                DisposeUnmanagedObjects();

                _hadDisposed = true;
            }
        }
    }
}