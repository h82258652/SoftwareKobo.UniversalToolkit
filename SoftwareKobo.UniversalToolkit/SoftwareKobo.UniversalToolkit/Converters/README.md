## BooleanInverseConverter
反转布尔值转换器。

## BooleanToVisibilityConverter
参考 http://winrtxamltoolkit.codeplex.com/SourceControl/latest#WinRTXamlToolkit/WinRTXamlToolkit.Shared/Converters/BooleanToVisibilityConverter.cs

## ItemClickEventArgsConverter
例子：
```XAML
<ListView IsItemClickEnabled="True">
	<ListView.Resources>
		<conv:ItemClickEventArgsConverter x:Key="ItemClickEventArgsConverter" />
	</ListView.Resources>
    <interactivity:Interaction.Behaviors>
		<core:EventTriggerBehavior EventName="ItemClick">
    	    <core:InvokeCommandAction InputConverter="{StaticResource ItemClickEventArgsConverter}" />
		</core:EventTriggerBehavior>
    </interactivity:Interaction.Behaviors>
</ListView>
```
```C#
DelegateCommand<Model>
```
那么 ViewModel 中 DelegateCommand 的类型直接声明为 Model 类型。

## StringFormatConverter
对转换的对象进行 String.Format 方法进行格式化，format 字符串使用 ConverterParameter。
例子：
```XAML
<Page xmlns:conv="using:SoftwareKobo.UniversalToolkit.Converters">
	<Page.Resources>
		<conv:StringFormatConverter x:Key="StringFormatConverter"/>
	</Page.Resources>
	<TextBlock Text="{Binding Path=Time,Converter={StaticResource StringFormatConverter},ConverterParameter='{}{0:yyyy-MM-dd}'}" />
</Page>
```

## UnixTimestampToDateTimeConverter
Unix 时间戳到 C# DateTime 类的双向转换器。