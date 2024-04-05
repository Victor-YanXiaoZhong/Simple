using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace Simple.WinUI.Controls.DataGridView.DataGridViewPrint
{
    public class DataGridViewPrint
    {
        /// <summary>ديتاگريدويويي كه همه چيز زير سر اونه</summary>
        private readonly System.Windows.Forms.DataGridView _dg;

        private readonly StringFormat _stringFormat;
        private readonly PageNumber _pg;
        private readonly PageTitle _pt;
        private readonly ColumnsHeaderSetting _chs;
        private RectangleF _rF;
        private Rectangle _r;
        private int _width;
        private int _top;
        private int _heigh;
        private SolidBrush _b;
        private int _colw, _rowh;
        private int _i = -1, _pageNumber;
        private bool _wrap;

        /// <summary>ساختن يك نمونه جديد از كلاس چاپ ديتاگريد</summary>
        /// <param name="dgpDataGridView">ديتاگريدي كه محتويات آن بايد چاپ شود</param>
        public DataGridViewPrint(System.Windows.Forms.DataGridView dgpDataGridView)
            : this(dgpDataGridView, new PrintDocument())
        {
        }

        /// <summary>ساختن يك نمونه جديد از كلاس چاپ ديتاگريد همراه با مشخص كردن سند چاپ</summary>
        /// <param name="dgpDataGridView">ديتاگريدي كه محتويات آن بايد چاپ شود</param>
        /// <param name="dgpDocument">سند چاپ</param>
        public DataGridViewPrint(System.Windows.Forms.DataGridView dgpDataGridView, PrintDocument dgpDocument = null)
        {
            _pg = new PageNumber();
            _pt = new PageTitle();
            _chs = new ColumnsHeaderSetting();
            if (dgpDocument == null) dgpDocument = new PrintDocument();
            PrinterSettings = new PrinterSettings();
            _stringFormat = new StringFormat();
            PrintDocument = dgpDocument;
            _dg = dgpDataGridView;
            _chs.Color = _dg.ColumnHeadersDefaultCellStyle.BackColor;
            _stringFormat.LineAlignment = StringAlignment.Center;
            _stringFormat.Alignment = StringAlignment.Near;
            _stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.FitBlackBox;
            PrinterSettings = PrintDocument.PrinterSettings;
            PageSetting = PrintDocument.DefaultPageSettings;
            Font = _pt.TitleFont = _chs.ColumnsHeaderFont = _dg.Font;
            PrintDocument.PrintPage += (pd_PrintPage);
        }

        /// <summary>دريافت و دريافت ديالوگ پيش نمايش چاپ</summary>
        public PrintPreviewDialog PrintPreviewDialog { get; set; }

        /// <summary>سند چاپ گزارش</summary>
        public PrintDocument PrintDocument { get; set; }

        /// <summary>تنظيمات صفحات گزارش</summary>
        public PageSettings PageSetting { get; set; }

        /// <summary>دريافت يا ست كردن تنظيمات پرينتر</summary>
        public PrinterSettings PrinterSettings { get; set; }

        /// <summary>فونت اصلي گزارش</summary>
        public Font Font { get; set; }

        /// <summary>دريافت يا ست كردن تنظيمات شماره صفحه گزارش</summary>
        public PageNumber PageNumber
        {
            get
            {
                return _pg;
            }
            set
            {
                _pg.StartFrom = value.StartFrom;
                _pg.Show = value.Show;
            }
        }

        /// <summary>دريافت يا ست كردن تنظيمات سر ستون فيلدهاي گزارش</summary>
        public ColumnsHeaderSetting ColumnsHeaderSetting
        {
            get
            {
                return _chs;
            }
            set
            {
                _chs.Color = value.Color;
                _chs.ColumnsHeaderFont = value.ColumnsHeaderFont;
            }
        }

        /// <summary>عناوين سرصفحه و پاصفحه</summary>
        public PageTitle PageTitle
        {
            get
            {
                return _pt;
            }
            set
            {
                _pt.Color = value.Color;
                _pt.HeaderStr = value.HeaderStr;
                _pt.SubTitle1 = value.SubTitle1;
                _pt.SubTitle2 = value.SubTitle2;
                _pt.TitleFont = value.TitleFont;
            }
        }

        /// <summary>بيان كننده چند خطي بودن متن هاي هر سلول</summary>
        public bool WrapText
        {
            get
            {
                return _wrap = (_stringFormat.FormatFlags & StringFormatFlags.NoWrap) != StringFormatFlags.NoWrap;
            }
            set
            {
                _wrap = value;
                if (_wrap)
                    _stringFormat.FormatFlags = StringFormatFlags.DirectionRightToLeft | StringFormatFlags.FitBlackBox;
                else
                    _stringFormat.FormatFlags = _stringFormat.FormatFlags | StringFormatFlags.NoWrap;
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            int left = PageSetting.Margins.Left;
            int right = PageSetting.Margins.Right;
            _width = PageSetting.Bounds.Width - right;
            _top = PageSetting.Margins.Top;
            _heigh = e.MarginBounds.Height - 180;
            _colw = 0;
            _rowh = 0;
            e.Graphics.Clear(Color.White);

            Pen headerPen = new Pen(Color.FromArgb(41, 30, 118)) { Width = 2f, LineJoin = LineJoin.Bevel };

            StringFormat hsc = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center,
                FormatFlags = StringFormatFlags.DirectionRightToLeft
            };

            Rectangle rectHeader = new Rectangle(left, _top, _width - left, 90);

            e.Graphics.DrawRectangle(headerPen, rectHeader);
            e.Graphics.DrawLine(headerPen, left + 130, _top, left + 130, _top + 90);
            e.Graphics.DrawLine(headerPen, _width - 100, _top, _width - 100, _top + 90);
            e.Graphics.DrawString(_pt.HeaderStr, new Font(_pt.TitleFont.Name, 13, FontStyle.Bold), new SolidBrush(_pt.Color), _width / 2 + left / 2, _top + 35, hsc);
            //تاريخ و شماره
            e.Graphics.DrawString(_pt.SubTitle1, _pt.TitleFont, new SolidBrush(_pt.Color), left + 120, _top + 25, _stringFormat);
            e.Graphics.DrawString(_pt.SubTitle2, _pt.TitleFont, new SolidBrush(_pt.Color), left + 120, _top + 65, _stringFormat);

            SizeF cellSize = new SizeF(0, 0);

            _top += 110;

            for (int j = 0; j < _dg.Columns.Count; j++)
            {
                if (e.Graphics.MeasureString(_dg.Columns[j].HeaderText, _chs.ColumnsHeaderFont, _dg.Columns[j].Width, _stringFormat).Height > cellSize.Height)
                    cellSize = e.Graphics.MeasureString(_dg.Columns[j].HeaderText, _chs.ColumnsHeaderFont, _dg.Columns[j].Width, _stringFormat);
            }
            cellSize.Height += 3;

            var totalWidth = _dg.Columns.GetColumnsWidth(DataGridViewElementStates.Visible);

            var startX = (_width - totalWidth + 100) / 2;

            var headFormat = _stringFormat.Clone() as StringFormat;
            headFormat.Alignment = StringAlignment.Center;
            //绘制标题
            for (int j = 0; j < _dg.Columns.Count; j++)
            {
                if (_dg.Columns[j].Visible == false)
                    continue;

                _rF = new RectangleF(startX, _rowh + _top, _dg.Columns[j].Width, cellSize.Height - 3);
                _r = new Rectangle(startX, _rowh + _top, _dg.Columns[j].Width, (int)cellSize.Height);
                startX += _dg.Columns[j].Width;

                _b = new SolidBrush(_chs.Color);

                e.Graphics.FillRectangle(_b, _r);
                e.Graphics.DrawString(_dg.Columns[j].HeaderText, _chs.ColumnsHeaderFont, Brushes.Black, _rF, headFormat);
                e.Graphics.DrawRectangle(Pens.Black, _r);
            }

            _rowh += (int)cellSize.Height;
            _colw = 0;

            //绘制行
            for (_i = _i + 1; _i < _dg.Rows.Count; _i++)
            {
                if (_dg.Rows[_i].IsNewRow)
                    continue;

                cellSize.Height = 0;

                for (int j = 0; j < _dg.Columns.Count; j++)
                {
                    var formattedValue = _dg[j, _i].FormattedValue;
                    if (formattedValue != null && e.Graphics.MeasureString(formattedValue.ToString(), Font, _dg.Columns[j].Width, _stringFormat).Height > cellSize.Height)
                        cellSize = e.Graphics.MeasureString(formattedValue.ToString(), Font, _dg.Columns[j].Width, _stringFormat);
                }
                //------------------------------
                cellSize.Height += 3;
                if (_rowh + (int)cellSize.Height > _heigh)
                {
                    _i -= 1;
                    break;
                }
                startX = (_width - totalWidth + 100) / 2;
                for (int j = 0; j < _dg.Columns.Count; j++)
                {
                    if (_dg.Columns[j].Visible == false)
                        continue;
                    _colw += _dg.Columns[j].Width;
                    if (_colw > (_width - e.MarginBounds.Left))
                        continue;
                    _rF = new RectangleF(startX, _rowh + _top, _dg.Columns[j].Width, cellSize.Height - 3);
                    _r = new Rectangle(startX, _rowh + _top, _dg.Columns[j].Width, (int)cellSize.Height);

                    startX += _dg.Columns[j].Width;

                    _b = _i % 2 == 0 ? new SolidBrush(_dg.DefaultCellStyle.BackColor) : new SolidBrush(_dg.AlternatingRowsDefaultCellStyle.BackColor);

                    e.Graphics.FillRectangle(_b, _r);

                    if (_dg.Columns[j].Visible && _dg[j, _i].Value != null)
                    {
                        var formattedValue = _dg[j, _i].FormattedValue;
                        if (formattedValue != null)
                            e.Graphics.DrawString(formattedValue.ToString(), Font, Brushes.Black, _rF, _stringFormat);
                    }

                    e.Graphics.DrawRectangle(new Pen(_dg.ForeColor), _r);
                }
                _colw = 0;
                _rowh += (int)cellSize.Height;
            }

            if (_pg.Show)
                e.Graphics.DrawString(_pageNumber.ToString(), _pt.TitleFont, Brushes.Black, _width / 2 + e.MarginBounds.Left / 2, (float)e.MarginBounds.Bottom - 30, hsc);

            if (_i < _dg.Rows.Count - 1)
            {
                _pageNumber += 1;
                e.HasMorePages = true;
            }
            else
                e.HasMorePages = false;
        }

        /// <summary>چاپ گزارش بدون نمايش</summary>
        public void Print()
        {
            _pageNumber = _pg.StartFrom;
            PrintDocument.DefaultPageSettings = PageSetting;
            PrintDocument.PrinterSettings = PrinterSettings;
            PrintDocument.Print();
        }

        /// <summary>نمايش گزارش</summary>
        public void PrinPreview()
        {
            _pageNumber = _pg.StartFrom;
            if (PrintPreviewDialog == null)
                PrintPreviewDialog = new PrintPreviewDialog();
            PrintPreviewDialog.Document = PrintDocument;
            PrintDocument.DefaultPageSettings = PageSetting;
            PrintDocument.PrinterSettings = PrinterSettings;
            PrintPreviewDialog.ShowDialog();
        }
    }

    /// <summary>.تنظيمات عنوان گزارش كه در بالاي صفحات نمايش داده مي شود</summary>
    public class PageTitle
    {
        /// <summary>عنوان صفحه</summary>
        public PageTitle()
        {
            Color = Color.Black;
        }

        /// <summary>.فونت عنوان گزارش. مقدار پيش فرض فونت ديتاگريد است</summary>
        public Font TitleFont { get; set; }

        /// <summary>.رنگ عنوان گزارش. مقدار پيش فرض رنگ سياه است</summary>
        public Color Color { get; set; }

        /// <summary>عنوان</summary>
        public string HeaderStr { get; set; }

        /// <summary>تاريخ1</summary>
        public string SubTitle1 { get; set; }

        /// <summary>تاريخ2</summary>
        public string SubTitle2 { get; set; }
    }

    /// <summary>تنظيمات شماره صفحه گزارش</summary>
    public class PageNumber
    {
        /// <summary>شماره صفحه</summary>
        public PageNumber()
        {
            Show = true;
            StartFrom = 1;
        }

        /// <summary>.عددي كه شماره صفحه از آن شروع شود. مقدار پيش فرض 1 است</summary>
        public int StartFrom { get; set; }

        /// <summary>
        /// .بيانگر اينكه آيا شماره صفحه در پايين گزارش نمايش داده شود يا خير؟ پيش فرض بلي است
        /// </summary>
        public bool Show { get; set; }
    }

    /// <summary>.تنظيمات سرستون فيلدهاي گزارش</summary>
    public class ColumnsHeaderSetting
    {
        /// <summary>.فونت سر ستون فيلدهاي گزارش. مقدار پيش فرض فونت ديتاگريد است</summary>
        public Font ColumnsHeaderFont { get; set; }

        /// <summary>.رنگ زمينه سر ستون فيلدهاي گزارش. مقدار پيش فرض رنگ زمينه ديتا گريد است</summary>
        public Color Color { get; set; }
    }
}