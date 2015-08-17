using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 委托命令。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Func<Task> _asyncExecute;
        private readonly Func<bool> _canExecute;
        private readonly Action _execute;

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="execute">命令动作。</param>
        public DelegateCommand(Action execute) : this(execute, null)
        {
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="asyncExecute">命令动作。</param>
        public DelegateCommand(Func<Task> asyncExecute) : this(asyncExecute, null)
        {
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="execute">命令动作。</param>
        /// <param name="canExecute">指示命令在某个状态下是否能够执行。</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this._execute = execute;
            this._canExecute = canExecute;
        }

        /// <summary>
        /// 初始化委托命令。
        /// </summary>
        /// <param name="asyncExecute">命令动作。</param>
        /// <param name="canExecute">指示命令在某个状态下是否能够执行。</param>
        public DelegateCommand(Func<Task> asyncExecute, Func<bool> canExecute)
        {
            if (asyncExecute == null)
            {
                throw new ArgumentNullException(nameof(asyncExecute));
            }

            this._asyncExecute = asyncExecute;
            this._canExecute = canExecute;
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
            return this._canExecute == null || this._canExecute();
        }

        /// <summary>
        /// 执行命令。
        /// </summary>
        /// <param name="parameter">参数。</param>
        public async void Execute(object parameter)
        {
            if (this._execute != null)
            {
                this._execute();
            }
            else
            {
                await this._asyncExecute();
            }
        }

        /// <summary>
        /// 通知命令是否允许执行发生变更。
        /// </summary>
        public virtual void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}