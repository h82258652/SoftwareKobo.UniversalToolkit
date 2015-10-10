## BooleanInverseConverter
反转布尔值转换器。

## BooleanToVisibilityConverter
参考 http://winrtxamltoolkit.codeplex.com/SourceControl/latest#WinRTXamlToolkit/WinRTXamlToolkit.Shared/Converters/BooleanToVisibilityConverter.cs

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