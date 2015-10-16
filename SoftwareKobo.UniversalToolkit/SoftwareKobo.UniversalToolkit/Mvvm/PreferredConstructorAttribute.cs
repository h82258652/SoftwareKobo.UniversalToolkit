using System;

namespace SoftwareKobo.UniversalToolkit.Mvvm
{
    /// <summary>
    /// 标注该 Attribute 在构造函数上，以指示 IoC 容器在创建对象时使用哪个构造函数。
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, Inherited = false, AllowMultiple = false)]
    public sealed class PreferredConstructorAttribute : Attribute
    {
    }
}