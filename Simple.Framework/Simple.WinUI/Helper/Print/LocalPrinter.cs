using System.Drawing.Printing;

namespace Simple.WinUI.Helper
{
    /// <summary>本地打印机辅助类</summary>
    public class LocalPrinter : PrintDocument
    {
        //打印相关
        private static string printerName = "";

        private static PageSetupDialog pageSetupDialog = new PageSetupDialog();

        //打印的图片
        private Image printImage = null;

        public LocalPrinter(string printerName = "")
        {
            PrinterName = string.IsNullOrEmpty(printerName) ? PrinterSettings.PrinterName : printerName;
            DocumentName = Environment.GetEnvironmentVariable("computername") + "_" + Environment.GetEnvironmentVariable("username");

            PrintController = new StandardPrintController();

            PrintPage += LocalPrinter_PrintPage;
            PrinterSettings.PrinterName = PrinterName;
        }

        /// <summary>打印机名称</summary>
		public string PrinterName
        {
            get { return printerName; }
            set
            {
                if (!GetLocalPrinters().Any(p => p == value))
                {
                    throw new Exception("无效打印机[" + value + "]");
                }
                printerName = value;
            }
        }

        private void LocalPrinter_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (printImage != null)
            {
                e.Graphics.DrawImage(printImage, e.MarginBounds);
            }
        }

        #region 公共方法

        private static Point Zoom(int x, int y, int maxWidth)
        {
            Point pt = new Point(x, y);
            while (pt.X > maxWidth)
            {
                pt.X /= 2;
                pt.Y /= 2;
            }
            return pt;
        }

        /// <summary>将像素单位转为毫米</summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        private static float DisplayToMm(int pixel)
        {
            //百分之一毫米
            var m = PrinterUnitConvert.Convert(pixel, PrinterUnit.Display, PrinterUnit.HundredthsOfAMillimeter);
            return m / 100.0F;
        }

        /// <summary>将毫米转为像素</summary>
        /// <param name="mm"></param>
        /// <returns></returns>
        private static int MmToDisplay(float mm)
        {
            int m = (int)(mm * 100);
            return PrinterUnitConvert.Convert(m, PrinterUnit.HundredthsOfAMillimeter, PrinterUnit.Display);
        }

        #endregion 公共方法

        //获取本机默认打印机名称
        public string DefaultPrinter()
        {
            return PrinterSettings.PrinterName;
        }

        /// <summary>获取打印机列表</summary>
        /// <returns></returns>
        public List<string> GetLocalPrinters()
        {
            List<string> fPrinters = new List<string>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (string fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }

        /// <summary>获取打印机纸张类型</summary>
        /// <param name="printerName">打印机名称</param>
        /// <param name="paperName">纸张类型名称</param>
        /// <returns></returns>
        public PaperSize GetPrintForm(string printerName, string paperName)
        {
            PaperSize paper = null;
            PrinterSettings printer = new PrinterSettings();
            printer.PrinterName = printerName;
            foreach (PaperSize ps in printer.PaperSizes)
            {
                if (ps.PaperName.ToLower() == paperName.ToLower())
                {
                    paper = ps;
                    break;
                }
            }
            return paper;
        }

        public void PrintSetting()
        {
            try
            {
                pageSetupDialog.PageSettings = new PageSettings();
                pageSetupDialog.ShowDialog();
                DefaultPageSettings = pageSetupDialog.PageSettings;
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");
            }
        }

        /// <summary>打印预览</summary>
        /// <param name="dt">要打印的DataTable</param>
        /// <param name="title">打印文件的标题</param>
        public void PrintPriview()
        {
            try
            {
                var printPriview = new PrintPreviewDialog
                {
                    Document = this,
                    WindowState = FormWindowState.Normal
                };
                printPriview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");
            }
        }

        public void PrintImage(string file, bool preview = false)
        {
            printImage = Image.FromFile(file);

            if (preview)
            {
                PrintPriview();
                return;
            }

            Print();
        }
    }
}