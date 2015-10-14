## Bootstrapper
用于简化 App 的启动、激活。
> 若无特别说明，则不兼容 Template10 的 Bootstrapper。

### 使用方式
修改 App.xaml。
```XAML
<toolkit:Bootstrapper x:Class="YourAppNamespace.App"
                      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                      xmlns:toolkit="using:SoftwareKobo.UniversalToolkit"
                      RequestedTheme="Light"></toolkit:Bootstrapper>
```
修改 App.xaml.cs
```C#
using SoftwareKobo.UniversalToolkit;

namespace YourAppNamespace
{
    sealed partial class App : Bootstrapper
    {
        public App()
        {
            // 设置默认导航页面。
            this.DefaultNavigatePage = typeof(MainView);
        }
    }
}
```

### 处理应用程序启动与激活
重写对应的虚方法。  
例如主启动（从主要磁贴或者应用程序列表等启动），可以重写 OnPrimaryStartAsync 方法：
```C#  
protected override Task OnPrimaryStartAsync(LaunchActivatedEventArgs args, AppStartInfo info)
{
    Debug.WriteLine("Primary Start");
	return Task.FromResult<object>(null);
}
```
又例如从关联协议启动：
```C#
protected override Task OnProtocolStartAsync(ProtocolActivatedEventArgs protocolArgs, AppStartInfo info)
{
	Debug.WriteLine("Protocol Start");
	// 修改协议启动导航的页面为 ProtocolPage。
	info.NavigatePage = typeof(ProtocolPage);
	return Task.FromResult<object>(null);
}
```

### 初始化应用程序
1. 对于非异步，耗时短，无特别要求（例如不是不能在构造函数中执行的）的操作，可以在 App 类的构造函数中执行；
2. 对于异步，耗时长，你可以选择使用扩展启动屏幕，并在其中执行初始化，具体参见使用扩展启动屏幕一节；
3. 对于异步，耗时短，你可以选择重写 PreStartAsync 方法（**该方法会在其它 Start 方法之前执行**）：
```C#
protected override async Task OnPreStartAsync(IActivatedEventArgs args, AppStartInfo info)
{
	// 模拟初始化。
    await Task.Delay(1000);
}
```
> 在扩展启动屏幕初始化与在 PreStartAsync 方法中初始化的区别：  
> **扩展启动屏幕初始化是仅在显示扩展启动屏幕时才会执行，而 PreStartAsync 则是每次应用程序启动激活都会执行。**  
> **但为了确保用户体验，请尽量使用扩展启动屏幕初始化。**

### 显示内存使用量
> 1. 可兼容 Template10，但是需要在构造函数以外的地方调用，具体参见下面。
> 2. 仅在 DEBUG 模式下显示。

修改 App.xaml.cs
```C#
using SoftwareKobo.UniversalToolkit;
using SoftwareKobo.UniversalToolkit.Extensions;

namespace YourAppNamespace
{
    sealed partial class App : Bootstrapper
    {
        public App()
        {
            // ...
            this.DebugSettings.EnableDisplayMemoryUsage();
        }
    }
}
```

若你的 App 类不继承自本项目的 Bootstrapper，则可以在 App 类构造函数以外的地方调用。
```C#
Application.Current.DebugSettings.EnableDisplayMemoryUsage();
```

### 使用扩展启动屏幕
参考 [Controls.ExtendedSplashScreenContent](https://github.com/h82258652/SoftwareKobo.UniversalToolkit3/blob/master/SoftwareKobo.UniversalToolkit/SoftwareKobo.UniversalToolkit/Controls/README.md) 一节。

### 显示新窗口
使用```ShowNewWindowAsync```方法来显示新窗口，并指定导航到哪个页面。  
该方法为异步方法，返回```Task<Window>```。  
若要调用方需要操作该窗口，请调用Window.Dispatcher来进行操作。

----------
## AppStartInfo
存放一些启动、激活相关的参数。各个 Start 方法都会包含该参数。

### NavigatePage 属性
获取或设置该次启动、激活应该导航到哪个页面。  
初始值为 Bootstrapper.Current.DefaultNavigatePage。  
你可以修改为其它页面类型来使该次启动、激活导航到哪个页面。  
**若设置为 null，则该次启动、激活不进行导航。**

### ExtendedSplashScreen 属性
获取或设置该次启动、激活应该使用哪个扩展启动屏幕。  
初始值为 Bootstrapper.Current.DefaultExtendedSplashScreen。  
若设置为 null 或者该方法返回 null 的时候，那么该次启动、激活不使用扩展启动屏幕。

### Parameter 属性
启动时的参数。

### IsShowInNewWindow 属性
设置该次启动、激活是否使用一个新窗口来显示内容。  
默认值为 false。
