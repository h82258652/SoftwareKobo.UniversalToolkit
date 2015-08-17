using Windows.ApplicationModel.Background;

namespace SoftwareKobo.UniversalToolkit.Extensions
{
    /// <summary>
    /// BackgroundTaskBuilder 扩展类。
    /// </summary>
    public static class BackgroundTaskBuilderExtensions
    {
        /// <summary>
        /// 将条件添加到后台任务。
        /// </summary>
        /// <param name="builder">后台任务。</param>
        /// <param name="conditions">多个 SystemCondition 对象的实例。</param>
        public static void AddConditions(this BackgroundTaskBuilder builder, params IBackgroundCondition[] conditions)
        {
            foreach (IBackgroundCondition condition in conditions)
            {
                builder.AddCondition(condition);
            }
        }
    }
}