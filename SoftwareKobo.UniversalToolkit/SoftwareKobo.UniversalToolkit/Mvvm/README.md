> # 若无特别说明，则不兼容其它 MVVM 框架。

## BindableBase
一个继承自 [Utils.DisposableObject](https://github.com/h82258652/SoftwareKobo.UniversalToolkit3/blob/master/SoftwareKobo.UniversalToolkit/SoftwareKobo.UniversalToolkit/Utils/README.md) 且实现 INotifyPropertyChanged 接口的抽象类。
与 MVVMLight 中的 ObserableObject 使用上无大区别。

## DelegateCommand
与 MVVMLight 中的 DelegateCommand 使用上无大区别。

## DelegateCommand&lt;T&gt;
DelegateCommand 的泛型实现。

## ViewModelBase
继承自 BindableBase，视图模型基类。

## 如何在 App 中使用该 MVVM 框架：
> View 必须实现 IView 接口。

View 代码：
```C#
public sealed class MainView : Page, IView
{
    public MainView()
    {
		this.InitializeComponent();
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

    public void ReceiveFromViewModel(dynamic parameter)
    {
		// 实现该方法自 IView 接口，以接收从 ViewModel 发送过来的消息。
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
	protected override void ReceiveFromView(dynamic parameter)
    {
		// 重写该方法以接收来自 View 的消息。
		Debug.WriteLine("View send:" + parameter);

		// 发送消息到对应的 View。
		SendToView("Send something to view.");
	}
} 
```

## IoC
一个简易的 IoC 容器。
> 若注入的对象有多个构造函数，请使用 ```PreferredConstructorAttribute``` 标注其中一个构造函数，以指示 IoC 容器应该如何创建对象。

## ViewModelLocatorBase
内部包含 IoC 容器的视图模型定位器。

## IncrementalLoadingCollection
> 兼容其它 MVVM 框架。

一个增量加载的集合，构造函数中需要传递一个 ```IncrementalItemSourceBase``` 对象。

## IncrementalItemSourceBase
> 兼容其它 MVVM 框架。

用于 ```IncrementalLoadingCollection```，指示应该如何加载数据和添加数据到集合当中。

## VerifiableBase
提供验证的 Model 基类，继承自 BindableBase。
例子：
```C#
public class Person : VerifiableBase
{
	[StringLength(5)]
    public string Name
    {
        get;
		set;
    }
}

public class MainViewModel : ViewModelBase
{
	public MainViewModel()
	{
		this.Person = new Person();
	}

    public Person Person
    {
        get;
		set;
    }
}
```
```XAML
<TextBox Text="{Binding Path=Person.Name,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged}" />
<!-- 显示 Name 属性的错误消息（若该属性有错误，则只显示第一条消息） -->
<TextBlock Text="{Binding Path=Person.Errors[Name]}" />
<!-- 显示 Person 对象所有属性的错误消息（每条消息以换行符分隔） -->
<TextBlock Text="{Binding Path=Person.Errors}" />
```
