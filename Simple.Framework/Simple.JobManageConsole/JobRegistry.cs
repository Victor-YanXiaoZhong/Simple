using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;
using FluentScheduler;

namespace Simple.JobManageConsole
{
    /// <summary>注册任务</summary>
    public class JobRegistry : Registry
    {
        /// <summary>自动注册实现IJob的接口的任务</summary>
        public JobRegistry()
        {
            Type baseType = typeof(IJobSchedule);
            var assemblies = GetAssemblyList();
            var jobTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t =>
                    t.IsClass && t.GetInterfaces().Any(m => baseType.IsAssignableFrom(m))).ToList();
                jobTypes.AddRange(types);
            }

            foreach (var jobSchedule in jobTypes)
            {
                var job = (IJobSchedule)Activator.CreateInstance(jobSchedule);
                var schedule = Schedule(job).WithName(job.JobName)
                    .NonReentrant();//禁止并行执行

                job.ScheduleConfig(schedule);
            }
        }

        /// <summary>获取所有的 程序集</summary>
        /// <param name="where"></param>
        /// <returns></returns>
        private static IEnumerable<Assembly> GetAssemblyList(Func<Assembly, bool> where = null)
        {
            #region 查找手动引用的程序集

            var assemblies = new List<Assembly>();
            if (assemblies.Count == 0)
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                if (entryAssembly == null) return assemblies;
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                assemblies = new List<Assembly> { entryAssembly }.Union(referencedAssemblies).ToList();

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
                        catch (Exception)
                        {
                        }
                    }
                }
                assemblies = AssemblyLoadContext.Default.Assemblies.Union(assemblies)
                    .Where(x => !x.FullName.Contains("Microsoft") && !x.FullName.Contains("System")).ToList();

                #endregion 将所有 dll 文件 重新载入 防止有未扫描到的 程序集
            }

            #endregion 查找手动引用的程序集

            return @where == null ? assemblies : assemblies.Where(@where);
        }
    }
}