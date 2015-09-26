namespace SoftwareKobo.UniversalToolkit
{
    #region 完全未动工

    // WrapPanel
    // DockPanel
    // AeroPanel https://github.com/JustinXinLiu/EffectsAndAnimationsWinComposition
    // 瀑布流面板 http://www.cnblogs.com/ms-uap/p/4715195.html
    // 汉堡按钮（绑定SplitView用于开关）
    // 下拉刷新、上推加载面板
    // CircleImage
    // Gif Image Control
    // WebP Image Control

    // ValidationSystem

    // ListViewBaseExtensions
    // 添加平滑滚动扩展方法。

    // 多窗口管理 http://www.cnblogs.com/tcjiaan/p/4748697.html
    // 多窗口管理2 http://www.cnblogs.com/lin277541/p/4835988.html
    /*
        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            // 创建新的视图
            CoreApplicationView newView  = CoreApplication.CreateNewView();

            ++newWindowCount;
            int newViewID = newWindowCount;

            // 初始化视图
            // 注意，必须在对应的线程上执行
            await newView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    // 获取视图ID，有两种方法
                    // 方法一：GetApplicationViewIdForWindow法，注意线程要对应
                    // Window必须是与当前视图关联的窗口
                    // int viewID = ApplicationView.GetApplicationViewIdForWindow(newView.CoreWindow);
                    // 方法二：最简单
                    // 因为当前执行的代码就在新视图的UI线程上的
                    // 所以GetForCurrentView所返回的就是刚创建的新视图
                    ApplicationView theView = ApplicationView.GetForCurrentView();

                    // 设置一下新窗口的标题（可选）
                    theView.Title = "new Windows" + newWindowCount.ToString();
                    // 必须记下视图ID
                    newViewID = theView.Id;
                    // 初始化视图的UI
                    MainPage newPage = new MainPage();
                    Window.Current.Content = newPage;
                    Window.Current.Activate();
                    // 必须调用Activate方法，否则视图不能显示
                    
                    注意：
                    在App类中，Window.Current获取的是主视图（程序刚启动时，至少要有一个视图，不然用户连毛都看不见了）所在的窗口。
                    而因为此处的代码是在新创建的视图的UI线程上执行的，故Window.Current自然获取的是新视图所在的窗口。
                   
});
            // 开始显示新视图
            bool b = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewID);

            if (b)
            {
                // 成功显示新视图
            }
            else
            {
                // 视图显示失败
            }
        }
    */

    // 异步方法异常捕捉 http://www.cnblogs.com/youngytj/p/4749004.html

    // 内置分享改进 http://www.cnblogs.com/ms-uap/p/4242326.html

    // 模板选择器 http://www.cnblogs.com/ms-uap/p/4201334.html

    // WeakAction 和 WeakAction<T> 然后改进整个框架特别是MVVM部分，使用WeakAction

    // MDL2
    // http://modernicons.io/segoe-mdl2/cheatsheet/
    // http://www.cnblogs.com/hebeiDGL/p/4386228.html
    // https://github.com/Windows-XAML/Template10/blob/master/Template10%20(Library)/Common/Mdl2.cs

    // AppxManifest.xml 辅助类
    // 后台任务helper or system（在Bootstrapper自动注册管理），后者否决，改为 T4 生成帮助类

    // Kinds of triggers

    #endregion 完全未动工

    #region 大概做了一半？

    // 商店 uri 帮助类 http://www.cnblogs.com/zhxilin/p/4819372.html
    // 需要将 Category 那两个函数改进。

    // VisualTreeHelper 扩展

    // 改进 Bootstrapper，特别是 NavigationState

    // 图片缓存系统
    // 优化性能

    #endregion 大概做了一半？

    #region 完工

    #endregion 完工
}