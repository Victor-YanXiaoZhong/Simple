using Simple.WinUI.Helper;

namespace Simple.WinUI.Forms
{
    /// <summary>右下角程序</summary>
    public partial class SysTrayAppConsole : BaseForm
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private Form mainPage;
        private SysTrayAppOption appOption;

        private bool trulyExiting = false;

        public SysTrayAppConsole(SysTrayAppOption appOption)
        {
            this.appOption = appOption;
            InitializeComponent();
            // 创建托盘菜单。
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("显示", Properties.Resources.display, showForm);
            trayMenu.Items.Add("退出", Properties.Resources.power, OnExit);

            // 创建托盘图标。
            trayIcon = new NotifyIcon();
            trayIcon.Text = appOption.AppTitle;
            trayIcon.Icon = appOption.AppIcon;
            trayIcon.DoubleClick += showForm;

            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
            Text = appOption.AppTitle;
            if (appOption.MainPage != null)
            {
                mainPage = appOption.MainPage;
                ShowMainForm(mainPage);
            }
            SizeChanged += (s, e) =>
            {
                if (mainPage != null)
                {
                    mainPage.WindowState = WindowState;
                }
            };

            FormClosing += SysTrayAppConsole_FormClosing;

            if (appOption.SetApplicationToStartup)
            {
                WinProcessHelper.SetApplicationToStartup(appOption.SetApplicationToStartup);
            }
        }

        private void showForm(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void SysTrayAppConsole_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (!trulyExiting)
            {
                e.Cancel = true;
                WindowState = FormWindowState.Normal;
                Hide();
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            if (this.Confirm("确认退出程序吗？"))
            {
                trulyExiting = true;
                Application.Exit();
            }
        }

        /// <summary>显示主要窗口</summary>
        /// <param name="mainPage"></param>
        private void ShowMainForm(Form mainPage)
        {
            mainPage.MdiParent = this;
            mainPage.FormBorderStyle = FormBorderStyle.None;
            mainPage.WindowState = FormWindowState.Maximized;
            mainPage.ControlBox = false;
            mainPage.Dock = DockStyle.Fill;
            mainPage.ShowInTaskbar = false;
            mainPage.Show();
        }
    }

    public class SysTrayAppOption
    {
        /// <summary>名称</summary>
        public string AppTitle { get; set; } = "最小化App";

        /// <summary>图标</summary>
        public Icon AppIcon { get; set; } = new Icon(SystemIcons.Application, 40, 40);

        /// <summary>程序开机启动</summary>
        public bool SetApplicationToStartup { get; set; } = true;

        /// <summary>显示时打开的页面</summary>
        public Form MainPage { get; set; }
    }
}