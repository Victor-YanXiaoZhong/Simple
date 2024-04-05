namespace Simple.Utils.Attributes
{
    /// <summary>自动注册每次实例化</summary>
    public class TransientAttribute : Attribute
    { }

    /// <summary>自动注册请求内单列</summary>
    public class ScopedAttribute : Attribute
    { }

    /// <summary>自动注册全局单列</summary>
    public class SingletonAttribute : Attribute
    { }
}