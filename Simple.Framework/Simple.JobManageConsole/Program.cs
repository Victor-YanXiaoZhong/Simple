using Simple.WinUI.Forms;

namespace Simple.JobManageConsole
{
    internal static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        [STAThread]
        private static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var option = new SysTrayAppOption
            {
                AppIcon = Properties.Resources.Logo,
                AppTitle = "调度控制台",
                MainPage = new JobManageConsoleForm()
            };

            Application.Run(new SysTrayAppConsole(option));
        }
    }

    public static class JobManageConsole
    {
        public static void Run(string name = "调度控制台", Icon logo = null)
        {
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