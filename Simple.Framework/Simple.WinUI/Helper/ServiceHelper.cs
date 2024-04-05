using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Helper
{
    public static class ServiceHelper
    {
        /// <summary>启动服务</summary>
        /// <param name="serviceName">服务名称</param>
        public static void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        /// <summary>停止服务</summary>
        /// <param name="serviceName">服务名称</param>
        public static void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

        /// <summary>获取服务安装路径</summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static string GetWindowsServiceInstallPath(string serviceName)
        {
            string key = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            string path = Registry.LocalMachine.OpenSubKey(key).GetValue("ImagePath").ToString();
            //替换掉双引号
            path = path.Replace("\"", string.Empty);

            FileInfo fi = new FileInfo(path);
            return fi.Directory.ToString();
        }

        /// <summary>判断服务是否存在</summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns></returns>
        public static bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>确认电脑上是否安装有某个程序</summary>
        /// <param name="softWareName">程序安装后的名称</param>
        /// <returns>true: 有安裝, false:沒有安裝</returns>
        public static bool CheckSoftWareInstallState(List<string> list)
        {
            Microsoft.Win32.RegistryKey uninstallNode =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths",
                Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree,
                System.Security.AccessControl.RegistryRights.ReadKey);
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("Path");
                if (displayName != null)
                {
                    if (list.Any(s => displayName.ToString().Contains(s)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>确认电脑上是否安装有某个程序,没有返回空，否则返回执行程序的完整地址</summary>
        /// <param name="dirName">程序安装后的目录名称</param>
        /// <param name="exeName">执行程序的名称</param>
        /// <returns></returns>
        public static string GetSoftWareInstallPos(string dirName, string exeName)
        {
            Microsoft.Win32.RegistryKey uninstallNode =
                Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths",
                Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree,
                System.Security.AccessControl.RegistryRights.ReadKey);
            foreach (string subKeyName in uninstallNode.GetSubKeyNames())
            {
                Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
                object displayName = subKey.GetValue("Path");
                if (displayName != null)
                {
                    if (displayName.ToString().Contains(dirName))
                    {
                        var exePath = displayName + exeName;
                        return File.Exists(exePath) ? exePath : null;
                    }
                }
            }
            return null;
        }
    }
}