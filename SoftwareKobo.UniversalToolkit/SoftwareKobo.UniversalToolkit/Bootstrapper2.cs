using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SoftwareKobo.UniversalToolkit
{
    public abstract class Bootstrapper2 : Application
    {
        protected Bootstrapper2()
        {
            this.Resuming += this.OnResuming;
            this.Suspending += async (sender, e) =>
           {
               SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
               try
               {
                   await this.OnSuspendingAsync(sender, e);
               }
               finally
               {
                   deferral.Complete();
               }
           };
        }

        #region 定稿

        public new Bootstrapper2 Current
        {
            get
            {
                return Application.Current as Bootstrapper2;
            }
        }

        public Frame RootFrame
        {
            get
            {
                return Window.Current.Content as Frame;
            }
        }

        protected virtual void OnResuming(object sender, object e)
        {
        }

        protected virtual Task OnSuspendingAsync(object sender, SuspendingEventArgs e)
        {
            return Task.FromResult<object>(null);
        }

        #endregion

        protected override void OnFileActivated(FileActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了文件类型关联。然后双击文件时，App 使用此方法作为入口。
            base.OnFileActivated(args);
        }

        protected override void OnFileOpenPickerActivated(FileOpenPickerActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了文件打开选取器。当其它 App 需要文件时，此 App 可以提供文件给其它 App。
            base.OnFileOpenPickerActivated(args);           
        }

        protected override void OnFileSavePickerActivated(FileSavePickerActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了文件保存选取器。当其它 App 需要保存文件时，此 App 可以代为执行保存的逻辑。
            base.OnFileSavePickerActivated(args);
        }

        protected override void OnCachedFileUpdaterActivated(CachedFileUpdaterActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了缓存文件更新程序。本 App 能够监视指定的 StorageFile，在其它 App 读取该文件或者在其它 App 保存该文件时使用该方法作为入口启动程序。
            base.OnCachedFileUpdaterActivated(args);
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了搜索。在其它 App 调用 charm bar 进行搜索时，能够看见本 App，若选择本 App 进行搜索，则通过此入口启动本程序。
            base.OnSearchActivated(args);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            // 应用通过点击磁贴、点击二级辅助磁贴、开始菜单列表打开时。
            base.OnLaunched(args);
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            // 应用在 Package.appxmanifest 中声明了共享目标。在进行系统共享时，如果数据格式符合，则会显示该 App。若选择该 App 进行共享，则通过此入口启动本程序。
            base.OnShareTargetActivated(args);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            // 除了 Launched 和其它 Activated 方法以外运行时作为入口。例如通过 Cortana Voice Command 启动。
            base.OnActivated(args);
        }

        protected virtual Task OnFileTypeAssociationLaunchedAsync()
        {
            return Task.FromResult<object>(null);
        }
    }
}