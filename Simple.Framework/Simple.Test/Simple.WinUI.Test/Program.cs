using Simple.WinUI.Forms;

namespace Simple.WinUI.Test
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
                AppTitle = "²âÊÔÐ¡Èý½Ç",
                MainPage = new MainPage()
            };

            Application.Run(new SysTrayAppConsole(option));
        }
    }
}