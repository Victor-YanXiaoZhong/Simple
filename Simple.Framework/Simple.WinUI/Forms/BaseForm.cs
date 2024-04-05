using System.ComponentModel;

namespace Simple.WinUI.Forms
{
    /// <summary>窗体基类接口</summary>
    [Description("窗体基类接口")]
    public interface IBaseForm
    {
    }

    /// <summary>扁平化窗体基类</summary>
    [Description("扁平化窗体基类")]
    public partial class BaseForm : Form, IBaseForm
    {
        #region 新增属性

        private bool borderEnabled = true;
        private int borderWidth = 1;

        private Color borderColor = Color.FromArgb(137, 158, 136);

        /// <summary>是否启用边框宽度</summary>
        [Description("是否启用边框宽度")]
        [DefaultValue(true)]
        [Browsable(true)]
        public bool BorderEnabled
        {
            get
            {
                return borderEnabled;
            }
            set
            {
                if (borderEnabled == value)
                    return;

                borderEnabled = value;
                this.Validate();
            }
        }

        /// <summary>边框宽度</summary>
        [Description("边框宽度")]
        [DefaultValue(1)]
        [Browsable(true)]
        public int BorderWidth
        {
            get
            {
                return this.BorderEnabled ? borderWidth : 0;
            }
            set
            {
                if (this.BorderEnabled == false || value < 0)
                    return;

                borderWidth = value;
                this.Validate();
            }
        }

        /// <summary>边框颜色</summary>
        [DefaultValue(typeof(Color), "137 ,158, 136")]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                if (borderColor == value)
                    return;

                borderColor = value;
                this.Invalidate();
            }
        }

        #endregion 新增属性

        #region 字段

        /// <summary>提示信息</summary>
        protected static ToolTip tte;

        #endregion 字段

        static BaseForm()
        {
            tte = new ToolTip();
            tte.BackColor = Color.FromArgb(240, 240, 240);
            tte.ForeColor = Color.FromArgb(109, 109, 109);
        }

        public BaseForm()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            AutoScaleMode = AutoScaleMode.Dpi;
            Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, (Byte)(134));
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // BaseForm
            AutoScaleDimensions = new SizeF(14F, 31F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(915, 694);
            Margin = new Padding(7, 8, 7, 8);
            Name = "BaseForm";
            Text = "BaseForm";
            ResumeLayout(false);
        }

        #region 重写

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00020000;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            this.DrawBorder(g);
        }

        #endregion 重写

        #region 公开方法

        /// <summary>隐藏提示信息</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void HideTip()
        {
            if (tte.Active)
            {
                tte.Hide(this);
            }
        }

        /// <summary>显示提示信息</summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void ShowTip(string text, Point point)
        {
            if (!String.IsNullOrEmpty(text))
            {
                tte.Show(text, this, point);
            }
        }

        #endregion 公开方法

        #region 私有方法

        /// <summary>绘制边框</summary>
        /// <param name="g"></param>
        private void DrawBorder(Graphics g)
        {
            if (this.BorderEnabled)
            {
                Pen border_pen = new Pen(this.BorderColor);
                Rectangle rect = new Rectangle(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width - 1, this.ClientRectangle.Height - 1);
                g.DrawRectangle(border_pen, rect);
                border_pen.Dispose();
            }
        }

        #endregion 私有方法
    }
}