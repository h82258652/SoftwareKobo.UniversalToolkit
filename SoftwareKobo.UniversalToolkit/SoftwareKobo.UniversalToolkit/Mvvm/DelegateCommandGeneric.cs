using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 委托命令。
    /// </summary>
    /// <typeparam name="T">参数类型。</typeparam>
    public class DelegateCommand<T> : ICommand
    {
        private readonly Func<T, Task> _asyncExecute;

        private readonly Func<T, bool> _canExecute;

        private readonly Action<T> _execute;

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="execute">命令动作。</param>
        public DelegateCommand(Action<T> execute) : this(execute, null)
        {
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="asyncExecute">命令动作。</param>
        public DelegateCommand(Func<T, Task> asyncExecute) : this(asyncExecute, null)
        {
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="execute">命令动作。</param>
        /// <param name="canExecute">指示命令在某个状态下是否能够执行。</param>
        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="asyncExecute">命令动作。</param>
        /// <param name="canExecute">指示命令在某个状态下是否能够执行。</param>
        public DelegateCommand(Func<T, Task> asyncExecute, Func<T, bool> canExecute)
        {
            if (asyncExecute == null)
            {
                throw new ArgumentNullException(nameof(asyncExecute));
            }

            _asyncExecute = asyncExecute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 指示当前状态是否允许命令执行。
        /// </summary>
        /// <param name="parameter">参数。</param>
        /// <returns>是否允许命令执行。</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        /// <summary>
        /// 执行命令。
        /// </summary>
        /// <param name="parameter">参数。</param>
        public async void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute((T)parameter);
            }
            else
            {
                await _asyncExecute((T)parameter);
            }
        }

        /// <summary>
        /// 通知命令是否允许执行发生变更。
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}