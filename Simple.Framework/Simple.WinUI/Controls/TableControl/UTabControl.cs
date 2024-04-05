using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

namespace Simple.WinUI.Controls.TableControl
{
    #region 带颜色的选项卡

    /// <summary>TabControl美化扩展</summary>
    [ToolboxItem(true)]
    [Description("TabControl美化扩展")]
    public class UTabControl : TabControl
    {
        #region 新增属性

        private Color backColor = Color.White;
        private Color tabPageBorderColor = Color.FromArgb(153, 204, 153);

        /// <summary>TabContorl背景颜色</summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("TabContorl背景颜色")]
        [DefaultValue(typeof(Color), "White")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public override Color BackColor
        {
            get { return this.backColor; }
            set
            {
                if (this.backColor == value)
                    return;
                this.backColor = value;
                base.Invalidate(true);
            }
        }

        /// <summary>TabContorl边框色</summary>
        [DefaultValue(typeof(Color), "153, 204, 153")]
        [Description("TabContorl边框色")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabPageBorderColor
        {
            get { return this.tabPageBorderColor; }
            set
            {
                if (this.tabPageBorderColor == value)
                    return;
                this.tabPageBorderColor = value;
                base.Invalidate(true);
            }
        }

        #region 圆角

        private int tabRadiusLeftTop = 0;
        private int tabRadiusRightTop = 0;

        private int tabRadiusRightBottom = 0;

        private int tabRadiusLeftBottom = 0;

        /// <summary>左上角圆角</summary>
        [Description("左上角圆角")]
        [DefaultValue(0)]
        public int TabRadiusLeftTop
        {
            get { return this.tabRadiusLeftTop; }
            set
            {
                if (this.tabRadiusLeftTop == value)
                    return;
                this.tabRadiusLeftTop = value;
                this.Invalidate();
            }
        }

        /// <summary>右上角圆角</summary>
        [Description("右上角圆角")]
        [DefaultValue(0)]
        public int TabRadiusRightTop
        {
            get { return this.tabRadiusRightTop; }
            set
            {
                if (this.tabRadiusRightTop == value)
                    return;
                this.tabRadiusRightTop = value;
                this.Invalidate();
            }
        }

        /// <summary>右下角圆角</summary>
        [Description("右下角圆角")]
        [DefaultValue(0)]
        public int TabRadiusRightBottom
        {
            get { return this.tabRadiusRightBottom; }
            set
            {
                if (this.tabRadiusRightBottom == value)
                    return;
                this.tabRadiusRightBottom = value;
                this.Invalidate();
            }
        }

        /// <summary>左下角圆角</summary>
        [Description("左下角圆角")]
        [DefaultValue(0)]
        public int TabRadiusLeftBottom
        {
            get { return this.tabRadiusLeftBottom; }
            set
            {
                if (this.tabRadiusLeftBottom == value)
                    return;
                this.tabRadiusLeftBottom = value;
                this.Invalidate();
            }
        }

        #endregion 圆角

        #region Tab选项

        private Color tabBackNormalColor = Color.FromArgb(153, 204, 153);
        private Color tabBackSelectedColor = Color.FromArgb(201, 153, 204, 153);

        private Color tabTextNormalColor = Color.FromArgb(255, 255, 255);

        private Color tabTextSelectedColor = Color.FromArgb(255, 255, 255);

        private StringAlignment tabTextAlignment = StringAlignment.Near;

        private bool textVertical = false;

        private Size tabImageSize = new Size(16, 16);

        /// <summary>Tab选项背景颜色(正常)</summary>
        [DefaultValue(typeof(Color), "153, 204, 153")]
        [Description("Tab选项背景颜色(正常)")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabBackNormalColor
        {
            get { return this.tabBackNormalColor; }
            set
            {
                if (this.tabBackNormalColor == value)
                    return;
                this.tabBackNormalColor = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab选项背景颜色(选中)</summary>
        [DefaultValue(typeof(Color), "201, 153, 204, 153")]
        [Description("Tab选项背景颜色(选中)")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabBackSelectedColor
        {
            get { return this.tabBackSelectedColor; }
            set
            {
                if (this.tabBackSelectedColor == value)
                    return;
                this.tabBackSelectedColor = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab选项文本颜色(正常)</summary>
        [DefaultValue(typeof(Color), "255, 255, 255")]
        [Description("Tab选项文本颜色(正常)")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabTextNormalColor
        {
            get { return this.tabTextNormalColor; }
            set
            {
                if (this.tabTextNormalColor == value)
                    return;
                this.tabTextNormalColor = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab选项文本颜色(选中)</summary>
        [DefaultValue(typeof(Color), "255, 255, 255")]
        [Description("Tab选项文本颜色(选中)")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabTextSelectedColor
        {
            get { return this.tabTextSelectedColor; }
            set
            {
                if (this.tabTextSelectedColor == value)
                    return;
                this.tabTextSelectedColor = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab选项文本对齐方式</summary>
        [DefaultValue(StringAlignment.Near)]
        [Description("Tab选项文本对齐方式")]
        public StringAlignment TabTextAlignment
        {
            get { return this.tabTextAlignment; }
            set
            {
                if (this.tabTextAlignment == value)
                    return;
                this.tabTextAlignment = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab选项文本是否垂直</summary>
        [Description("Tab选项文本是否垂直")]
        [DefaultValue(false)]
        public bool TextVertical
        {
            get { return this.textVertical; }
            set
            {
                if (this.textVertical == value)
                    return;
                this.textVertical = value;
                this.Invalidate();
            }
        }

        /// <summary>tab选项图标大小</summary>
        [Description("左下角圆角")]
        [DefaultValue(typeof(Size), "16,16")]
        public Size TabImageSize
        {
            get { return this.tabImageSize; }
            set
            {
                if (this.tabImageSize == value)
                    return;
                this.tabImageSize = value;
                this.Invalidate();
            }
        }

        #endregion Tab选项

        #region 关闭

        private bool tabCloseShow = false;
        private Size tabCloseSize = new Size(10, 10);

        private Color tabCloseBackColor = Color.FromArgb(255, 255, 255);

        /// <summary>Tab选项关闭按钮是否显示</summary>
        [Description("Tab选项关闭按钮是否显示")]
        [DefaultValue(false)]
        public bool TabCloseShow
        {
            get { return this.tabCloseShow; }
            set
            {
                if (this.tabCloseShow == value)
                    return;
                this.tabCloseShow = value;
                this.Invalidate();
            }
        }

        /// <summary>Tab关闭按钮Size</summary>
        [DefaultValue(typeof(Size), "10, 10")]
        [Description("Tab关闭按钮Size")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Size TabCloseSize
        {
            get { return this.tabCloseSize; }
            set
            {
                if (this.tabCloseSize == value)
                    return;
                this.tabCloseSize = value;
                base.Invalidate();
            }
        }

        /// <summary>Tab关闭按钮背景颜色</summary>
        [DefaultValue(typeof(Color), "255, 255, 255")]
        [Description("Tab关闭按钮背景颜色")]
        [Editor(typeof(ColorEditor), typeof(UITypeEditor))]
        public Color TabCloseBackColor
        {
            get { return this.tabCloseBackColor; }
            set
            {
                if (this.tabCloseBackColor == value)
                    return;
                this.tabCloseBackColor = value;
                base.Invalidate();
            }
        }

        #endregion 关闭

        #endregion 新增属性

        #region 重写属性

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected new bool DesignMode
        {
            get
            {
                if (this.GetService(typeof(IDesignerHost)) != null || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                {
                    return true;   //界面设计模式
                }
                else
                {
                    return false;//运行时模式
                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(300, 200);
            }
        }

        [DefaultValue(TabDrawMode.OwnerDrawFixed)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new TabDrawMode DrawMode
        {
            get { return TabDrawMode.OwnerDrawFixed; }
            set { base.DrawMode = TabDrawMode.OwnerDrawFixed; }
        }

        [DefaultValue(TabSizeMode.Fixed)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new TabSizeMode SizeMode
        {
            get { return TabSizeMode.Fixed; }
            set { base.SizeMode = TabSizeMode.Fixed; }
        }

        #endregion 重写属性

        #region 字段

        private int tabImageMarginLeft = 4;

        private int pd = 2;

        private int preNextBtnWidth = 40;

        #endregion 字段

        public UTabControl()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.DrawMode = TabDrawMode.Normal;
            this.SizeMode = TabSizeMode.Fixed;
        }

        #region 重写

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            #region 绘制所有Tab选项

            SolidBrush back_normal_sb = new SolidBrush(this.TabBackNormalColor);
            SolidBrush back_selected_sb = new SolidBrush(this.TabBackSelectedColor);
            SolidBrush text_normal_sb = new SolidBrush(this.TabTextNormalColor);
            SolidBrush text_selected_sb = new SolidBrush(this.TabTextSelectedColor);
            StringFormat text_sf = new StringFormat() { Alignment = this.TabTextAlignment, LineAlignment = StringAlignment.Center, Trimming = StringTrimming.EllipsisCharacter };
            Pen close_pen = new Pen(this.TabCloseBackColor, 2) { StartCap = LineCap.Round, EndCap = LineCap.Round };

            Rectangle tab_rect = this.GetTabRectangle();
            Region client_region = null;
            Region tabitem_region = null;
            if (this.Alignment == TabAlignment.Top || this.Alignment == TabAlignment.Bottom)
            {
                client_region = g.Clip.Clone();
                tabitem_region = new Region(tab_rect);
                g.Clip = tabitem_region;
            }
            for (int i = 0; i < this.TabCount; i++)
            {
                Rectangle rect = this.GetTabRect(i);
                GraphicsPath path = GraphicsPathManager.TransformCircular(rect, this.TabRadiusLeftTop, this.TabRadiusRightTop, this.TabRadiusRightBottom, this.TabRadiusLeftBottom);

                #region 绘制Tab选项背景颜色

                g.FillPath((i == this.SelectedIndex) ? back_selected_sb : back_normal_sb, path);

                #endregion 绘制Tab选项背景颜色

                #region 绘制Tab选项图片

                if (this.ImageList != null && this.ImageList.Images.Count > 0)
                {
                    Image img = null;
                    if (this.TabPages[i].ImageIndex > -1)
                    {
                        img = this.ImageList.Images[this.TabPages[i].ImageIndex];
                    }
                    else if (this.TabPages[i].ImageKey.Trim().Length > 0)
                    {
                        img = this.ImageList.Images[this.TabPages[i].ImageKey];
                    }
                    if (img != null)
                    {
                        g.DrawImage(img, rect.X + this.tabImageMarginLeft, rect.Y + (rect.Height - this.TabImageSize.Height) / 2, this.TabImageSize.Width, this.TabImageSize.Height);
                    }
                }

                #endregion 绘制Tab选项图片

                #region 绘制Tab选项文本

                if (this.ImageList != null && ((this.TabPages[i].ImageIndex > -1) || (this.TabPages[i].ImageKey.Trim().Length > 0)))
                {
                    rect = new Rectangle(rect.Left + this.TabImageSize.Width + this.tabImageMarginLeft * 2, rect.Top, rect.Width - this.TabImageSize.Width - this.tabImageMarginLeft * 2, rect.Height);
                }

                if (this.TextVertical)
                {
                    string text = this.TabPages[i].Text;
                    float sum = 0;
                    SizeF text_size = g.MeasureString(text, this.Font, new PointF(), text_sf);
                    for (int j = 0; j < text.Length; j++)
                    {
                        RectangleF char_rect = new RectangleF(this.Padding.X + rect.X, this.Padding.Y + rect.Y + sum, text_size.Width, text_size.Height + 1);
                        g.DrawString(text.Substring(j, 1), this.Font, (i == this.SelectedIndex) ? text_selected_sb : text_normal_sb, char_rect, text_sf);
                        sum += text_size.Height + 1;
                    }
                }
                else
                {
                    g.DrawString(this.TabPages[i].Text, this.Font, (i == this.SelectedIndex) ? text_selected_sb : text_normal_sb, rect, text_sf);
                }

                #endregion 绘制Tab选项文本

                #region 绘制关闭按钮

                if (this.TabCloseShow && this.GetIsShowCloseButton(i))
                {
                    RectangleF close_rect = this.GetTabCloseRectangle(i);
                    g.DrawLine(close_pen, new PointF(close_rect.X, close_rect.Y), new PointF(close_rect.Right, close_rect.Bottom));
                    g.DrawLine(close_pen, new PointF(close_rect.Right, close_rect.Y), new PointF(close_rect.Left, close_rect.Bottom));
                }

                #endregion 绘制关闭按钮
            }

            if (tabitem_region != null)
            {
                g.Clip = client_region;
                tabitem_region.Dispose();
            }

            if (back_normal_sb != null)
                back_normal_sb.Dispose();
            if (back_selected_sb != null)
                back_selected_sb.Dispose();
            if (text_normal_sb != null)
                text_normal_sb.Dispose();
            if (text_selected_sb != null)
                text_selected_sb.Dispose();
            if (text_sf != null)
                text_sf.Dispose();
            if (close_pen != null)
                close_pen.Dispose();

            #endregion 绘制所有Tab选项

            #region 设置TabPage内容页边框色

            if (this.TabCount > 0)
            {
                Pen border_pen = new Pen(this.TabPageBorderColor, 1);
                Rectangle borderRect = this.TabPages[0].Bounds;
                borderRect.Inflate(1, 1);
                g.DrawRectangle(border_pen, borderRect);
                border_pen.Dispose();
            }

            #endregion 设置TabPage内容页边框色
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!this.DesignMode)
            {
                if (this.TabCloseShow && e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    #region 关闭

                    Rectangle tab_rect = this.GetTabRectangle();
                    if (tab_rect.Contains(e.Location))
                    {
                        for (int i = 0; i < this.TabPages.Count; i++)
                        {
                            if (this.GetTabCloseRectangle(i).Contains(e.Location) && this.GetIsShowCloseButton(i))
                            {
                                int index = 0;
                                if (i >= this.TabPages.Count - 1)
                                {
                                    index = i - 1;
                                    if (i < 0)
                                    {
                                        i = 0;
                                    }
                                }
                                else
                                {
                                    index = i + 1;
                                }
                                this.SelectedIndex = index;
                                this.TabPages.Remove(this.TabPages[i]);
                                return;
                            }
                        }
                    }

                    #endregion 关闭
                }
            }

            base.OnMouseClick(e);
        }

        #endregion 重写

        #region 私有方法

        /// <summary>获取Tab选项区Rectangle</summary>
        /// <returns></returns>
        private Rectangle GetTabRectangle()
        {
            int tabitem_width = this.TabPages.Count * this.ItemSize.Width;
            if (this.Alignment == TabAlignment.Top || this.Alignment == TabAlignment.Bottom)
            {
                if (tabitem_width > this.ClientRectangle.Width - this.pd * 2)
                {
                    tabitem_width = this.ClientRectangle.Width - this.preNextBtnWidth - this.pd * 2;
                }
            }
            int y = 0;
            if (this.Alignment == TabAlignment.Top)
                y = this.ClientRectangle.Y + this.pd;
            else if (this.Alignment == TabAlignment.Bottom)
                y = this.ClientRectangle.Bottom - this.pd - this.ItemSize.Height;

            return new Rectangle(this.ClientRectangle.X + this.pd, y, tabitem_width, this.ItemSize.Height + this.pd);
        }

        /// <summary>获取Tab选项关闭按钮Rectangle</summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private RectangleF GetTabCloseRectangle(int index)
        {
            Rectangle rect = this.GetTabRect(index);
            RectangleF close_rect = new RectangleF(rect.Right - 10 - this.TabCloseSize.Width, rect.Y + (rect.Height - this.TabCloseSize.Height) / 2f, this.TabCloseSize.Width, this.TabCloseSize.Height);
            return close_rect;
        }

        /// <summary>是否不显示关闭按钮</summary>
        /// <param name="index"></param>
        private bool GetIsShowCloseButton(int index)
        {
            if (this.TabPages[index].Tag != null && this.TabPages[index].Tag.ToString() == "不显示关闭按钮")
            {
                return false;
            }

            return true;
        }

        #endregion 私有方法
    }

    #endregion 带颜色的选项卡
}