# SoftwareKobo.UniversalToolkit

## 项目目标：
一个用于简化 Windows 10 App 开发的工具库。

## 如何使用：
要使用该工具库，你需要使用 nuget 程序包管理器进行安装。
https://www.nuget.org/packages/SoftwareKobo.UniversalToolkit/
> 目前由于[技术原因](http://docs.nuget.org/create/uwp-create#directory-structure)，简化多语言的 T4 模板需要手动复制到项目当中。https://raw.githubusercontent.com/h82258652/SoftwareKobo.UniversalToolkit/master/SoftwareKobo.UniversalToolkit/SoftwareKobo.UniversalToolkit.T4/LocalizedStrings.tt

## 项目进度：
目前处于 beta。  
本项目不会有 roadmap 之类的东西，因为需要取决于是否能实现和我有没有空。但是，只要有用的，我都会考虑加进该项目。  
或者你可以参见：https://github.com/h82258652/SoftwareKobo.UniversalToolkit/blob/master/SoftwareKobo.UniversalToolkit/SoftwareKobo.UniversalToolkit/TODO.cs  
里面列出了我认为有用的，虽然不一定会实现（多半因为技术原因，或者我懒=_=）。

## 这个工具库里面主要有什么：
1. 简化 App 启动、激活逻辑；
2. 一些常用的 Converter；
3. 一些有用的控件；
4. 一些有用的 State Trigger；
5. VisualTreeHelper 扩展；
6. 一个超小型的 MVVM 框架；
7. 简化多语言的 T4 模板；
8. 其它乱七八糟的东西，具体可以参见每个文件夹下的 **README.md**。

## 兼容其它库吗？
### [Template10](https://github.com/Windows-XAML/Template10)
不兼容，但你仍然可使用该工具库的一部分组件，例如 State Trigger。但仍然不推荐一起使用。

### [MVVMLight](http://www.mvvmlight.net/)、[MvvmCross](https://github.com/MvvmCross/MvvmCross) 等 MVVM 框架 
兼容，但请勿与本工具库的 MVVM 部分混合使用。

### [WinRTXamlToolkit](http://winrtxamltoolkit.codeplex.com/)
完全兼容。

> 如果你不清楚其它库是否与本库兼容的话，可以在 issue 中提问。

## 请问能去掉 Json.NET 的依赖吗？
抱歉，这点不会，因为 Json.NET 已经是 .NET 平台下大家公认的 Json 实现。对比起 .NET Framework 里各个平台不一样的 Json 实现，Json.NET 统一的 API 你觉得不更好吗？

## 我在使用过程中遇到问题，怎么办？
你可以在这个项目的 issue 里提出，我会尽量解答。
> 另外有好的建议也可以在 issue 提出或者 pull request。但是我会视乎是否有用来决定是否改进或合并。

## 最后说一下使用注意
### 由于本项目目前还不稳定，请审慎考虑是否需要在你的项目中使用，并且本项目中的 API 可能会在以后发生改变。