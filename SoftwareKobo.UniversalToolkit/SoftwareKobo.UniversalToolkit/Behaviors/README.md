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
> Command 参数是 ItemClickEventArgs。建议配合 ItemClickEventArgsConverter 使用。

## ListViewBaseScrollBehavior
ListView、GridView 等滚动方向行为。

## TimeoutAction
延时命令。
IsConcurrent 指是否并发。为 false 情况下，若来了新的执行请求，而上次请求还没开始的话，那么则会结束掉上次请求。true 的情况下则不相互影响。