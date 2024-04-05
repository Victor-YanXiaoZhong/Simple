using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.Button
{
    #region 图片按钮

    /// <summary>图片按钮</summary>
    public class UPictrueButton : System.Windows.Forms.PictureBox
    {
        #region 构造

        /// <summary>构造</summary>
        public UPictrueButton()
        {
            //首先开启双缓冲，防止闪烁
            //双缓冲的设置 具体参数含义参照msdn的ControlStyles枚举值
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        #endregion 构造

        #region 图片按钮标题

        /// <summary>图片按钮标题</summary>
        private string _title = "";

        [Browsable(true)]
        [Description("图片按钮标题")]
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                this.Invalidate();
            }
        }

        #endregion 图片按钮标题

        #region 图片按钮标题颜色

        /// <summary>图片按钮标题颜色</summary>
        private Color _titleForeColor = Color.White;

        [Browsable(true)]
        [Description("标题颜色")]
        public Color TitleForeColor
        {
            get { return _titleForeColor; }
            set
            {
                _titleForeColor = value;
                this.Invalidate();
            }
        }

        #endregion 图片按钮标题颜色

        #region 图片按钮默认图片

        /// <summary>图片按钮默认图片</summary>
        private Bitmap _defaultBitmap;

        [Browsable(true)]
        [Description("图片按钮默认图片")]
        public Bitmap DefaultBitmap
        {
            get { return _defaultBitmap; }
            set
            {
                _defaultBitmap = value;
                if (_defaultBitmap != null)
                {
                    _defaultBitmap.MakeTransparent(Color.FromArgb(192, 192, 192));
                    this.Invalidate();
                }
            }
        }

        #endregion 图片按钮默认图片

        #region 图片按钮鼠标选中后的图片

        /// <summary>图片按钮鼠标选中后的图片</summary>
        private Bitmap _mouseOverImage;

        [Browsable(true)]
        [Description("图片按钮鼠标选中后的图片")]
        public Bitmap MouseOverImage
        {
            get { return _mouseOverImage; }
            set
            {
                _mouseOverImage = value;
                if (_mouseOverImage != null)
                {
                    _mouseOverImage.MakeTransparent(Color.FromArgb(192, 192, 192));
                    this.Invalidate();
                }
            }
        }

        #endregion 图片按钮鼠标选中后的图片

        #region 图片按钮是否选中

        /// <summary>图片按钮是否选中</summary>
        private bool _isSelcted = false;

        [Browsable(true)]
        [Description("图片按钮是否选中")]
        public bool IsSelcted
        {
            get { return _isSelcted; }
            set
            {
                _isSelcted = value;
                this.Invalidate();
            }
        }

        #endregion 图片按钮是否选中

        #region 图片按钮标题选中后的颜色

        /// <summary>图片按钮选中颜色</summary>
        private Color _selectTitleForeColor = Color.Black;

        [Browsable(true)]
        [Description("图片按钮选中后的标题颜色")]
        public Color SelectTitleForeColor
        {
            get
            {
                return _selectTitleForeColor;
            }
            set
            {
                _selectTitleForeColor = value;
                this.Invalidate();
            }
        }

        #endregion 图片按钮标题选中后的颜色

        #region 图片按钮标题字体默认12

        /// <summary>图片按钮标题字体默认12</summary>
        private Font _titleFont = new Font(FontFamily.GenericMonospace, 12.0f);

        [Browsable(true)]
        [Description("图片按钮标题字体默认GenericMonospace 12F")]
        public Font TitleFont
        {
            get
            {
                return _titleFont;
            }
            set
            {
                _titleFont = value;
                this.Invalidate();
            }
        }

        #endregion 图片按钮标题字体默认12

        #region 重写图片按钮尺寸改变事件

        /// <summary>重写图片按钮尺寸改变事件</summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }

        #endregion 重写图片按钮尺寸改变事件

        #region 鼠标是否进入

        /// <summary>鼠标是否进入</summary>
        private bool _isMouseEnter = false;

        #endregion 鼠标是否进入

        #region 重写鼠标进入事件

        /// <summary>重写鼠标进入事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseEnter = true;
            base.OnMouseEnter(e);
            this.Invalidate();
        }

        #endregion 重写鼠标进入事件

        #region 重写鼠标离开事件

        /// <summary>重写鼠标离开事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseEnter = false;
            base.OnMouseLeave(e);
            this.Invalidate();
        }

        #endregion 重写鼠标离开事件

        #region 重绘图片

        /// <summary>重绘图片</summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            if (this._defaultBitmap != null && this._mouseOverImage != null)
            {
                if (IsSelcted)
                {
                    pe.Graphics.DrawImage(this._mouseOverImage, 0, 0, this.Width, this.Height);
                }
                else
                {
                    if (_isMouseEnter)
                    {
                        pe.Graphics.DrawImage(this._mouseOverImage, 0, 0, this.Width, this.Height);
                    }
                    else
                    {
                        pe.Graphics.DrawImage(this._defaultBitmap, 0, 0, this.Width, this.Height);
                    }
                }
                if (_title != "")
                {
                    Font font = this._titleFont;
                    SizeF size = pe.Graphics.MeasureString(_title, font);
                    PointF pointF = new PointF((this.Width - size.Width) / 2, (this.Height - size.Height) / 2);
                    if (_isMouseEnter || _isSelcted)
                    {
                        pe.Graphics.DrawString(_title, font, new SolidBrush(_selectTitleForeColor), pointF);
                    }
                    else
                    {
                        pe.Graphics.DrawString(_title, font, new SolidBrush(_titleForeColor), pointF);
                    }
                }
            }
            pe.Dispose();
        }

        #endregion 重绘图片
    }

    #endregion 图片按钮
}