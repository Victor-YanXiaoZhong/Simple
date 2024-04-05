using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Simple.WinUI.Forms;
using Simple.WinUI.Helper;

namespace Simple.Tool.Pages
{
    public partial class WebToolPage : BaseForm
    {
        private WebView2 webView;

        public WebToolPage()
        {
            InitializeComponent();

            webView = new WebView2()
            {
                Dock = DockStyle.Fill,
            };
            this.Controls.Add(webView);
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.NavigationStarting += (s, e) => { MaskHelper.Show(this, "页面加载中"); };
            webView.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
            webView.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            webView.CoreWebView2.Navigate("https://c.runoob.com/");
        }

        private void CoreWebView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            MaskHelper.Hide(this);
        }

        private void CoreWebView2_NewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            MaskHelper.Show(this, "页面加载中");
            e.Handled = true;
            webView.CoreWebView2.Navigate(e.Uri);
        }

        private void 收藏ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            webView.CoreWebView2.Navigate(e.ClickedItem.Tag.ToString());
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
    }
}