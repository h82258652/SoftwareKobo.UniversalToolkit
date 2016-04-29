using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// MessageDialog 扩展类。
    /// </summary>
    public static class MessageDialogExtensions
    {
        private static TaskCompletionSource<MessageDialog> _currentDialogShowRequest;

        /// <summary>
        /// 指示当前是否可以显示对话框。
        /// </summary>
        public static bool IsCurrentShowable => _currentDialogShowRequest == null;

        /// <summary>
        /// 添加一个命令到对话框。
        /// </summary>
        /// <param name="dialog">对话框。</param>
        /// <param name="label">命令文本。</param>
        /// <param name="action">命令动作。</param>
        /// <returns>对话框。</returns>
        /// <exception cref="ArgumentNullException"><c>dialog</c> 为空。</exception>
        public static MessageDialog AddCommand(this MessageDialog dialog, string label, Action action)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            var command = new UICommand(label, commandAction => action?.Invoke());
            dialog.Commands.Add(command);

            return dialog;
        }

        /// <summary>
        /// 将对话框加入显示队列。
        /// </summary>
        /// <param name="dialog">对话框。</param>
        /// <returns>指示点击了对话框的哪个命令。</returns>
        /// <exception cref="ArgumentNullException"><c>dialog</c> 为空。</exception>
        public static async Task<IUICommand> ShowAsyncEnqueue(this MessageDialog dialog)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            while (_currentDialogShowRequest != null)
            {
                await _currentDialogShowRequest.Task;
            }

            _currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
            var request = _currentDialogShowRequest;
            var result = await dialog.ShowAsync();
            _currentDialogShowRequest = null;
            request.SetResult(dialog);

            return result;
        }

        /// <summary>
        /// 如果可以的话，则显示对话框。
        /// </summary>
        /// <param name="dialog">对话框。</param>
        /// <returns>指示点击了对话框的哪个命令。</returns>
        /// <exception cref="ArgumentNullException"><c>dialog</c> 为空。</exception>
        public static async Task<IUICommand> ShowAsyncIfPossible(this MessageDialog dialog)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }
            if (_currentDialogShowRequest != null)
            {
                return null;
            }

            _currentDialogShowRequest = new TaskCompletionSource<MessageDialog>();
            var request = _currentDialogShowRequest;
            var result = await dialog.ShowAsync();
            _currentDialogShowRequest = null;
            request.SetResult(dialog);

            return result;
        }
    }
}