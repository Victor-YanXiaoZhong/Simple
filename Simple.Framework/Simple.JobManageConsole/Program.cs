using Simple.WinUI.Forms;
using Simple.WinUI.Helper;

namespace Simple.JobManageConsole
{
    internal static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
            JobManageConsole.Run();
        }
    }

    public static class JobManageConsole
    {
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            ExceptionLog(ex);
        }

        private static void GlobalExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            ExceptionLog(e.Exception);
        }

        private static void ExceptionLog(Exception ex, string text = "出现未处理的异常")
        {
            UIHelper.AlertError(ex, text);
        }

        public static void Run(string name = "调度控制台", Icon logo = null)
        {
            // 添加全局异常处理
            Application.ThreadException += new ThreadExceptionEventHandler(GlobalExceptionHandler);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            ApplicationConfiguration.Initialize();

            var option = new SysTrayAppOption
            {
                AppIcon = Properties.Resources.Logo,
                AppTitle = name,
                MainPage = new JobManageConsoleForm()
            };

            if (logo != null) option.AppIcon = logo;

            Application.Run(new SysTrayAppConsole(option));
        }
    }
}