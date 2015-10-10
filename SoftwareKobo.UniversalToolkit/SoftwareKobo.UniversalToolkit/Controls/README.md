## DisplayMemoryUsage
显示内存使用量。

## DockPanel
参见 WPF 的 DockPanel。
#未完成。

## ExtendedSplashScreenContent
> 不兼容 Template10。

用于承载扩展启动屏幕内容。
例子：
新建一个用户控件，假如叫 CustomExtendedSplashScreen，然后修改代码。
XAML 代码
```XAML
<controls:ExtendedSplashScreenContent x:Class="YourAppNamespace.CustomExtendedSplashScreen"
									  xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                                      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                                      xmlns:controls="using:SoftwareKobo.UniversalToolkit.Controls">
	<Grid>
		<TextBlock Text="正在启动"
                   TextAlignment="Center" 
				   VerticalAlignment="Bottom"
				   Margin="0,0,0,100" />
	</Grid>
</<controls:ExtendedSplashScreenContent>
```
CS 代码
```C#
public sealed partial class CustomExtendedSplashScreen : ExtendedSplashScreenContent
{
	public CustomExtendedSplashScreen()
	{
		this.InitializeComponent();
		this.Loaded += async (sender, e) => {
			// 初始化。
			await Task.Delay(1000);

			// 完成后调用 Finish 方法以结束扩展启动屏幕。
			this.Finish();
		};
	}
}
```
修改 App 类。
```C#
public sealed partial class App : Bootstrapper
{
    public App()
	{
		this.DefaultMainPage = typeof(MainView);

		this.DefaultExtendedSplashScreen = () => new CustomExtendedSplashScreen();
	}
} 
```
然后就可以使用扩展启动屏幕了。

## FullWindowPopup
一个占据满当前窗口的 Popup。
### FullWindowPopup.AttachedPopup 附加属性
能够附着到一个 XAML 元素上，便于编写。
例子：
```XAML
<Grid>
	<controls:FullWindowPopup.AttachedPopup>
		<controls:FullWindowPopup>
			<Grid>
				<TextBlock Text="{Binding Path=Name}" />
			</Grid>
		</controls:FullWindowPopup>
	</controls:FullWindowPopup.AttachedPopup>
</Grid>
```
> 注意：请勿修改该形式下的 FullWindowPopup 的 DataContext 属性。

## ReflectionPanel
参见 http://www.cnblogs.com/h82258652/p/4839649.html

## WrapPanel
参见 WPF 的 DockPanel。
#未完成。