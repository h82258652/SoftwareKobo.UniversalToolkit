# SoftwareKobo.UniversalToolkit
# 这个类库仍然在开发当中，暂未完成，请勿在生产环境中使用。
# 即将发布第一 Beta 版，敬请期待（届时你可以使用 nuget 包管理器进行安装）

## 项目目标：
一个用于简化开发 Windows Universal App 的工具库。

## 项目进度：
目前仍在开发，暂时还不可用，第一 Beta 版即将发布。（目前还需要一些工作）

## 这个工具库里面主要有什么：
1. 简化 App 启动、激活逻辑；
2. 一些常用的 Converter；
3. 一些有用的控件；
4. 一些有用的 State Trigger；
5. VisualTreeHelper 扩展；
6. 一个小型的 MVVM 框架；
7. 简化多语言的 T4 模板；
8. 7.其它乱七八糟的东西，具体可以参见每个文件夹下的 README.md。（但仍然在编写中，请原谅）

## 兼容其它库吗？
### [Template10](https://github.com/Windows-XAML/Template10)
不兼容，但你仍然可使用该工具库的一部分组件，例如 State Trigger。但个人仍然不推荐一起使用。

### [MVVMLight](http://www.mvvmlight.net/)、[MvvmCross](https://github.com/MvvmCross/MvvmCross) 等 MVVM 框架 
兼容，但请勿与本工具库的 MVVM 部分混合使用。

### [WinRTXamlToolkit](http://winrtxamltoolkit.codeplex.com/)
完全兼容。

## 请问能去掉 Json.NET 的依赖吗？
抱歉，这点不会，因为 Json.NET 已经是 .NET 平台下大家公认的 Json 实现。对比起 .NET Framework 里各个平台不一样的 Json 实现，Json.NET 统一的 API 你觉得不更好吗？
> 目前本工具库还依赖 Unity，但以后会尽量移除掉。

## 我在使用过程中遇到问题，怎么办？
你可以在这个项目的 issue 里提出，我会尽量解答。
> 另外有好的建议也可以在 issue 提出或者 pull request。

## 最后说一下使用注意
### 由于本项目目前还不稳定，因此不建议在生产环境中使用，并且本项目中的 API 可能会在以后发生改变。