> # 若无特别说明，则不兼容其它 MVVM 框架。

## BindableBase
一个继承自 [Utils.DisposableObject](https://github.com/h82258652/SoftwareKobo.UniversalToolkit3/blob/master/SoftwareKobo.UniversalToolkit/SoftwareKobo.UniversalToolkit/Utils/README.md) 且实现 INotifyPropertyChanged 接口的抽象类。
与 MVVMLight 中的 ObserableObject 使用上无大区别。

## DelegateCommand
与 MVVMLight 中的 DelegateCommand 使用上无大区别。

## DelegateCommand<T>
DelegateCommand 的泛型实现。

## ViewModelBase
继承自 BindableBase，视图模型基类。

## ViewModelLocatorBase
视图模型定位器基类。
# 暂未完成。

## 如何在 App 中使用该 MVVM 框架：
> **本 MVVM 框架需要视图 View 与视图模型 ViewModel 名称必须遵守**

> **ViewName + "Model" 等于 ViewModelName**

> **即假设有 View 叫 MainView，则 ViewModel 必须叫 MainViewModel。**

View 代码：
```C#
public sealed class MainView : Page, IView
{
    public MainView()
    {
    }

	protected override void OnNavigatedTo(NavigationEventArgs e)
    {
		// 注册该 View 到通信管理器。
        Messenger.Register(this);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
		// 从通信管理器注销该 View。
        Messenger.Unregister(this);
    }

    public void ReceiveFromViewModel(ViewModelBase originSourceViewModel, object parameter)
    {
		// 该方法实现 IView 接口，以接收从 ViewModel 发送过来的消息。
		// originSourceViewModel 参数应该尽量不用，仅在特殊情况下，例如程序运行时存在多个该 View 所对应的 ViewModel 的实例时需要用作检查才使用。
		Debug.WriteLine("ViewModel send:" + parameter);
    }

	public void Button_Click(object sender, RoutedEventArgs e)
	{
		// 发送消息到对应的 ViewModel。
		this.SendToViewModel("Send something to view model.");
	}
}
```
ViewModel 代码：
```C#
public class MainViewModel : ViewModelBase
{
	protected override void ReceiveFromView(FrameworkElement originSourceView, object parameter)
    {
		// 重写该方法以接收来自 View 的消息。
		// originSourceView 同上面 View 的情况，应尽量不用。
		Debug.WriteLine("View send:" + parameter);

		// 发送消息到对应的 View。
		SendToView("Send something to view.");
	}
} 
```
----------
## VerifiableBase
# 暂未完成。

## IncrementalLoadingCollection
> 兼容其它 MVVM 框架。

# 暂未完成。

## IIncrementalItemSource
> 兼容其它 MVVM 框架。

# 暂未完成。