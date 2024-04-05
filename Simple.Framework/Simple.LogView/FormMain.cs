using Simple.WinUI.Forms;

namespace Simple.LogView
{
    public partial class FormMain : BaseForm
    {
        private LogHand logHand;

        public FormMain()
        {
            InitializeComponent();
        }

        private void LogHand_MessageEv(string name, string message)
        {
            this.Invoke(new Action(() =>
            {
                try
                {
                    txbLog.SelectionStart = txbLog.TextLength;
                    txbLog.SelectionLength = 0;
                    txbLog.SelectionColor = Color.Blue;
                    txbLog.AppendText("***************************************************" + Environment.NewLine);
                    txbLog.SelectionColor = txbLog.ForeColor;

                    txbLog.AppendText($"[{name}]：{message}{Environment.NewLine}");
                }
                catch (Exception ex)
                {
                }
            }));
        }

        protected override void OnShown(EventArgs e)
        {
            logHand = new LogHand();
            logHand.MessageEv += LogHand_MessageEv;
            toolStripStatus.Text = $"接收日志服务已启动，端口：{10001}";
            WinUI.WinApi.SetForegroundWindow(this.Handle);
        }
    }
}