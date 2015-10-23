## ItemClickBehavior
用于 ListView、GridView 以及其它继承自 ListViewBase。
例子：
```XAML
<ListView>
	<Interactivity:Interaction.Behaviors>
		<Core:ItemClickBehavior>
			<Core:InvokeCommandAction Command="{Binding ClickCommand}" />
		</Core:ItemClickBehavior>
	</Interactivity:Interaction.Behaviors>
</ListView>
```
> Command 获取的参数直接是 ClickItem 而不是 EventArgs。