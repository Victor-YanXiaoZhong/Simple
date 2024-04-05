using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Simple.RunService
{
    /// <summary>服务启动文件帮助类</summary>
    internal class Helper
    {
        private static readonly Dictionary<string, string> commandDic = new()
        {
            {"1","install" },{"2","start" },{"3","restart" },{"4","stop" },{"5","test" },
            {"6","status" },{"7","uninstall" }
        };

        /// <summary>获取输入值</summary>
        /// <param name="lable"></param>
        /// <param name="require"></param>
        /// <returns></returns>
        private static string ReadLine(string lable, bool require = false)
        {
            Console.Write(lable + ":");
            var read = Console.ReadLine();
            if (require && (read == null || read.Length == 0))
            {
                ReadLine(lable, require);
            }
            return read;
        }

        /// <summary>设置服务启动类型（CMD）</summary>
        /// <param name="command">命令名称</param>
        /// <param name="message">设置失败时的原因</param>
        /// <returns>true：执行未报错</returns>
        private static Boolean RunCMD(String command, out String message)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = @"cmd.exe",
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
                Thread.Sleep(3000);
                message = process.StandardOutput.ReadToEnd();
                Write(message);
                message = process.StandardError.ReadToEnd();
                Write(message);
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
                RunAsService();
            }
            return result;
        }

        /// <summary>设置配置文件</summary>
        /// <returns></returns>
        private static service SetConfig()
        {
            Write("初次运行将进行服务配置：", ConsoleColor.DarkGreen);

            var xmlOp = new service();
            xmlOp.name = ReadLine("请输入服务名称", true);
            xmlOp.description = ReadLine("请输入服务描述");
            xmlOp.executable = ReadLine("请输入服务运行程序", true);
            xmlOp.arguments = ReadLine("请输入服务参数");
            return xmlOp;
        }

        private static string GetXmlProp(string prop)
        {
            var xmlContent = File.ReadAllText("Service.xml");
            var pattern = @$"<{prop}>(.*?)<\/{prop}>";
            var match = Regex.Match(xmlContent, pattern);
            return match.Groups[1].Value;
        }

        public static void Write(object msg, ConsoleColor color = ConsoleColor.White)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// 服务运行 只支持单个 yourapp.exe start // 安装并启动服务 yourapp.exe stop // 停止并删除服务 yourapp.exe logs //
        /// 控制台输出服务的日志 yourapp.exe logs filter="key words" // 控制台输出服务的日志
        /// </summary>
        /// <param name="serviceModule"></param>
        public static void RunAsService()
        {
            var xmlOp = WriteServiceXml();
            Write("请选择要进行的操作:", ConsoleColor.DarkGreen);
            Console.WriteLine("----->1.注册服务");
            Console.WriteLine("----->2.启动服务");
            Console.WriteLine("----->3.重启服务");
            Console.WriteLine("----->4.停止服务");
            Console.WriteLine("----->5.测试服务");
            Console.WriteLine("----->6.服务状态查看");
            Console.WriteLine("----->7.卸载服务");
            Console.WriteLine("----->8.打开配置文件");
            var commandType = ReadLine("请输入数字", true);

            if (commandType == "8")
            {
                if (File.Exists("Service.xml"))
                {
                    Process.Start("notepad.exe", "Service.xml");
                }
                else
                {
                    Write("请先注册生成配置文件Service.xml");
                }
                return;
            }

            if (commandDic.TryGetValue(commandType, out var command))
            {
                RunCMD($@"Service.exe {command} Service.xml", out string message);
                Console.WriteLine(message);
                Console.ReadKey();
            }
            else
            {
                Write("请输入正确的数字进行操作", ConsoleColor.DarkYellow);
                RunAsService();
                Console.ReadKey();
            }
        }

        /// <summary>生成配置文件</summary>
        /// <param name="serviceOptions"></param>
        /// <param name="args"></param>
        public static service WriteServiceXml()
        {
            if (File.Exists("Service.xml"))
            {
                var xmlOp = new service();
                xmlOp.executable = GetXmlProp("executable").Replace("%BASE%", AppDomain.CurrentDomain.BaseDirectory);
                xmlOp.arguments = GetXmlProp("arguments");
                return xmlOp;
            }

            var serviceOptions = SetConfig();

            var xml =
    $@"<service>
    <id>{serviceOptions.id}</id>
    <name>{serviceOptions.name}</name>
    <description>{serviceOptions.description}</description>
    <executable>%BASE%\{serviceOptions.executable}</executable>
    <arguments>{serviceOptions.arguments}</arguments>
    <logpath>%BASE%\logs</logpath>
    <log mode=""roll-by-time"">
      <pattern>yyyyMMdd</pattern>
    </log>
</service>
";
            File.WriteAllText("Service.xml", xml);
            Console.WriteLine("已生成服务配置文件Service.xml");
            return serviceOptions;
        }
    }
}