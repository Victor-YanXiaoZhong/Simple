using System.Reflection;
using System.Runtime.Loader;

namespace Simple.Utils.Helper
{
    /// <summary>运行时帮助类</summary>
    public class RuntimeHelper
    {
        /// <summary>获取项目程序集，排除所有的系统程序集(Microsoft.***、System.***等)、Nuget下载包</summary>
        /// <param name="loadUnreferenceDll">是否扫描目录下所有dll，包括未引入到项目的</param>
        /// <returns></returns>
        public static IList<Assembly> GetAllAssemblies(bool loadUnreferenceDll = false)
        {
            if (loadUnreferenceDll)
            {
                #region 将所有 dll 文件 重新载入 防止有未扫描到的 程序集

                var paths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                    .Where(w => w.EndsWith(".dll") && !w.Contains("Microsoft"))
                    .Select(w => w)
                 ;
                foreach (var item in paths)
                {
                    if (File.Exists(item))
                    {
                        try
                        {
                            Assembly.Load(AssemblyLoadContext.GetAssemblyName(item));
                        }
                        catch (Exception) { }
                    }
                }

                #endregion 将所有 dll 文件 重新载入 防止有未扫描到的 程序集
            }

            var list = Assembly.GetEntryAssembly().GetReferencedAssemblies().Select(Assembly.Load)
                .Where(p => !p.FullName.Contains("Microsoft.")
                && !p.FullName.Contains("System.")).ToList();
            return list;
        }

        /// <summary>获取指定名称程序集</summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public static Assembly GetAssembly(string assemblyName)
        {
            return GetAllAssemblies().FirstOrDefault(assembly => assembly.FullName.Contains(assemblyName));
        }

        /// <summary>获取所有类型</summary>
        /// <returns></returns>
        public static IList<Type> GetAllTypes()
        {
            var list = new List<Type>();
            foreach (var assembly in GetAllAssemblies())
            {
                var typeInfos = assembly.DefinedTypes;
                foreach (var typeInfo in typeInfos)
                {
                    list.Add(typeInfo.AsType());
                }
            }
            return list;
        }

        public static IList<Type> GetTypesByAssembly(string assemblyName)
        {
            var list = new List<Type>();
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(assemblyName));
            var typeInfos = assembly.DefinedTypes;
            foreach (var typeInfo in typeInfos)
            {
                list.Add(typeInfo.AsType());
            }
            return list;
        }

        public static Type GetImplementType(string typeName, Type baseInterfaceType)
        {
            return GetAllTypes().FirstOrDefault(t =>
            {
                if (t.Name == typeName &&
                    t.GetTypeInfo().GetInterfaces().Any(b => b.Name == baseInterfaceType.Name))
                {
                    var typeInfo = t.GetTypeInfo();
                    return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericType;
                }
                return false;
            });
        }
    }
}