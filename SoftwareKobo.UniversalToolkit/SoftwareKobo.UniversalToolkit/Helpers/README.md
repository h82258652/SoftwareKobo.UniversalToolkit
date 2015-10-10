## DeviceFamilyHelper
获取当前设备类型。

## HardwareButtonsHelper
获取当前设备是否存在手机上的后退键。

## NavigationBackHelper
简化页面后退导航的逻辑。
例子：
```C#
using SoftwareKobo.UniversalToolkit.Helpers;

public sealed class DetailPage : Page
{ 
	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		this.Frame.RegisterNavigateBack();
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		this.Frame.UnregisterNavigateBack();
	}
}
```
那么在 Desktop 下，窗口左上角将会出现后退按钮，Mobile 下按后退键将会后退。
另外也可以自己定义后退键的逻辑：
```C#
using SoftwareKobo.UniversalToolkit.Helpers;

public sealed class DetailPage : Page
{ 
	protected override void OnNavigatedTo(NavigationEventArgs e)
	{
		this.Frame.RegisterNavigateBack(async () =>
		{
			if (Frame.CanGoBack)
			{
				await storyboard.BeginAsync();
				if (Frame.CanGoBack)
				{
					Frame.GoBack();
				}
			}
		});
	}

	protected override void OnNavigatedFrom(NavigationEventArgs e)
	{
		this.Frame.UnregisterNavigateBack();
	}
}
```
那么这样将会播放完 Storyboard 再后退。