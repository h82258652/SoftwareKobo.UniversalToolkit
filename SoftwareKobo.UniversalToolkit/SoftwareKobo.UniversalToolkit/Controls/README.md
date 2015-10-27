## AdaptiveCollectionView
一个能在 ListView、GridView 变换的面板。
例子：
```XAML
<Grid xmlns:controls="using:SoftwareKobo.UniversalToolkit.Controls">
	<VisualStateManager.VisualStateGroups>
		<VisualStateGroup>
			<VisualState x:Name="narrow">
				<VisualState.Setters>
					<Setter Target="View.Mode"
							Value="List" />
				</VisualState.Setters>
				<VisualState.StateTriggers>
					<AdaptiveTrigger MinWindowWidth="0" />
				</VisualState.StateTriggers>
			</VisualState>
			<VisualState x:Name="wide">
				<VisualState.Setters>
					<Setter Target="View.Mode"
							Value="Grid" />
				</VisualState.Setters>
				<VisualState.StateTriggers>
					<AdaptiveTrigger MinWindowWidth="800" />
				</VisualState.StateTriggers>
			</VisualState>
		</VisualStateGroup>
	</VisualStateManager.VisualStateGroups>  
	<controls:AdaptiveCollectionView x:Name="View"
	    							 ItemsSource="{Binding Path=YourCollection}"
	    							 Mode="List">
	    <controls:AdaptiveCollectionView.ItemTemplate>
			<DataTemplate>
				<Grid Width="150"
					  Height="150"
                      Background="SkyBlue">
					<ContentControl Content="{Binding}"/>
				</Grid>
			</DataTemplate>
		</controls:AdaptiveCollectionView.ItemTemplate>
		<controls:AdaptiveCollectionView.ItemContainerStyle>
			<Style TargetType="controls:AdaptiveCollectionViewItem">
            	<Setter Property="Margin"
                	    Value="10" />
            </Style>
        </controls:AdaptiveCollectionView.ItemContainerStyle>
    </controls:AdaptiveCollectionView>
</Grid>
```
那么在窗口宽度小于 800 的时候，将会使用 ListView 模式，而大于等于 800 时，则使用 GridView 模式。

## AeroPanel
结合 Win2D 仿照 Aero 效果的控件。
例子：
```XAML
<Grid>
    <Grid.ColumnDefinitions>
		<ColumnDefinition Width="Auto" />
		<ColumnDefinition Width="*" />
	</Grid.ColumnDefinitions>
	<Image x:Name="content"
           Grid.ColumnSpan="2"
           Source="your test img uri"
           Stretch="Fill">
	<controls:AeroPanel RenderElement="{Binding ElementName=content">
        <!--在这里放 AeroPanel 的内容。-->
    </controls:AeroPanel>
</Grid>
```
> 1、AeroPanel 默认会有一个透明浅蓝色遮罩效果，要去除可以修改 MaskColor 为 Transparent。
> 2、部分情况下可能 AeroPanel 背景渲染和 RenderElement 的位置不正确，可以调用 AeroPanel.UpdateRender 方法刷新背景渲染。

## DisplayMemoryUsage
显示内存使用量。

## DockPanel
参见 WPF 的 DockPanel。

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
			// 模拟初始化。
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
> 注意：请勿手动修改 FullWindowPopup 的 DataContext 属性。

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

## HamburgerButton
汉堡按钮。
例子：
```XAML
<controls:HamburgerButton IsChecked="{Binding ElementName=view,Path=IsPaneOpen,Mode=TwoWay}" Height="37" Content="这是一个汉堡按钮"></controls:HamburgerButton>
<SplitView x:Name="view"></SplitView>
```

## ReflectionPanel
参见 http://www.cnblogs.com/h82258652/p/4839649.html

## WrapPanel
参见 WPF 的 DockPanel。