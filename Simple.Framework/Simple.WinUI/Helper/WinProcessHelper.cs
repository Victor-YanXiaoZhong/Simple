using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI
{
    /// <summary>windows进程帮助类</summary>
    public class WinProcessHelper
    {
        /// <summary>激活与当前进程同名且已运行的进程，没有已运行的进程返回null</summary>
        /// <returns>与指定进程同名且已运行的进程对象或null</returns>
        public static void ActiveRunning()
        {
            Process fromProcess = Process.GetCurrentProcess();
            Process destProcess = GetRunning(fromProcess);
            if (destProcess == null) { return; }
            try
            {
                ShowAndActive(destProcess.MainWindowHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show(fromProcess.ProcessName + "已运行，但激活出错 " + ex.Message, "错误", MessageBoxButtons.OK,
                    MessageBoxIcon.Stop, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        /// <summary>获取与指定进程同名且已运行的进程，没有已运行的进程返回null</summary>
        /// <param name="fromProcess">来源经常</param>
        /// <returns>与指定进程同名且已运行的进程对象或null</returns>
        public static Process GetRunning(Process fromProcess)
        {
            Int32 processId = fromProcess.Id;
            String processPath = fromProcess.MainModule.FileName;
            Process[] processes = Process.GetProcessesByName(fromProcess.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id != processId) { return process; }
            }
            return null;
        }

        /// <summary>显示并激活指定的窗口</summary>
        /// <param name="windowHandle">窗体句柄</param>
        public static void ShowAndActive(IntPtr windowHandle)
        {
            WinApi.ShowWindowAsync(windowHandle, 1);
            WinApi.SetForegroundWindow(windowHandle);
        }

        /// <summary>设置服务启动类型（CMD）</summary>
        /// <param name="command">命令名称</param>
        /// <param name="message">设置失败时的原因</param>
        /// <returns>true：执行未报错</returns>
        public static Boolean RunCMD(String command, out String message)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = @"C:\Windows\System32\cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            Boolean result;
            Process process = new Process() { StartInfo = startInfo };

            try
            {
                process.Start();
                process.StandardInput.WriteLine(command);
                process.StandardInput.WriteLine("exit");
                message = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                message = ex.Message + Environment.NewLine + ex.GetType().ToString();
            }
            finally
            {
                process.Close();
            }
            return result;
        }
    }
}