using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Simple.WinUI.Controls.SplitContainer
{
    #region 分割容器控件

    /// <summary>WWW.CSharpSkin.COM 分割容器控件</summary>
    public class USplitContainer : System.Windows.Forms.SplitContainer
    {
        #region 成员

        private readonly object eventCollapseClick = new object();

        /// <summary>成员</summary>
        private CollapsePanel _collapsePanel = CollapsePanel.PanelOne;

        private SplitPanelState _splitPanelState = SplitPanelState.Expanded;

        private SplitContainerState _splitContainerState;

        private int _lastSplitDistance;

        private int _minSize;

        private SplitButtonSatate _splitButtonSatate;

        internal SplitPanelState SpliterPanelState
        {
            get { return _splitPanelState; }
            set
            {
                if (_splitPanelState != value)
                {
                    switch (value)
                    {
                        case SplitPanelState.Expanded:
                            Expand();
                            break;

                        case SplitPanelState.Collapsed:
                            Collapse();
                            break;
                    }
                    _splitPanelState = value;
                }
            }
        }

        internal SplitContainerState SplitContainerState
        {
            get { return _splitContainerState; }
            set
            {
                if (_splitContainerState != value)
                {
                    _splitContainerState = value;
                    base.Invalidate(CollapseRect);
                }
            }
        }

        public CollapsePanel CollapsePanel
        {
            get { return _collapsePanel; }
            set
            {
                if (_collapsePanel != value)
                {
                    Expand();
                    _collapsePanel = value;
                }
            }
        }

        [Browsable(false)]
        public new BorderStyle BorderStyle
        {
            get
            {
                return BorderStyle.None;
            }
            set
            {
                BorderStyle = BorderStyle.None;
            }
        }

        #endregion 成员

        #region 分割箭头颜色

        /// <summary>分割箭头颜色</summary>
        public Color _arrowColor = Color.FromArgb(80, 136, 228);

        [Browsable(true)]
        [Description("分割箭头颜色")]
        public Color ArrowColor
        {
            get
            {
                return _arrowColor;
            }
            set
            {
                _arrowColor = value;
                this.Invalidate();
            }
        }

        #endregion 分割箭头颜色

        #region 分隔条颜色

        /// <summary>分隔条颜色</summary>
        public Color _splitColor = Color.Silver;

        [Browsable(true)]
        [Description("分隔条颜色")]
        public Color SplitColor
        {
            get
            {
                return _splitColor;
            }
            set
            {
                _splitColor = value;
                this.Invalidate();
            }
        }

        #endregion 分隔条颜色

        #region 边框颜色

        /// <summary>边框颜色</summary>
        private Color _BorderColor = Color.Silver;

        [Browsable(true)]
        [Description("边框颜色")]
        public Color BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                this.Invalidate();
            }
        }

        #endregion 边框颜色

        #region 构造

        /// <summary>构造</summary>
        public USplitContainer() : base()
        {
            //首先开启双缓冲，防止闪烁
            //双缓冲的设置 具体参数含义参照msdn的ControlStyles枚举值
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            _lastSplitDistance = base.SplitterDistance;
            SplitterWidth = 10;
            Size = new Size(350, 150);
        }

        #endregion 构造

        #region 编辑事件

        /// <summary>编辑事件</summary>
        public event EventHandler CollapseClick
        {
            add { base.Events.AddHandler(eventCollapseClick, value); }
            remove { base.Events.RemoveHandler(eventCollapseClick, value); }
        }

        #endregion 编辑事件

        #region 返回默认展开宽度

        /// <summary>返回默认展开宽度</summary>
        protected int DefaultCollapseWidth
        {
            get { return 80; }
        }

        #endregion 返回默认展开宽度

        #region 返回默认箭头宽度

        /// <summary>返回默认箭头宽度</summary>
        protected int DefaultArrowWidth
        {
            get { return 16; }
        }

        #endregion 返回默认箭头宽度

        #region 返回展开区域

        /// <summary>返回展开区域</summary>
        protected Rectangle CollapseRect
        {
            get
            {
                if (_collapsePanel == CollapsePanel.None)
                {
                    return Rectangle.Empty;
                }
                Rectangle rect = base.SplitterRectangle;
                if (base.Orientation == Orientation.Horizontal)
                {
                    rect.X = (base.Width - DefaultCollapseWidth) / 2;
                    rect.Width = DefaultCollapseWidth;
                }
                else
                {
                    rect.Y = (base.Height - DefaultCollapseWidth) / 2;
                    rect.Height = DefaultCollapseWidth;
                }
                return rect;
            }
        }

        #endregion 返回展开区域

        #region 缩起来

        /// <summary>缩起来</summary>
        public void Collapse()
        {
            if (_collapsePanel != CollapsePanel.None && _splitPanelState == SplitPanelState.Expanded)
            {
                _lastSplitDistance = base.SplitterDistance;
                if (_collapsePanel == CollapsePanel.PanelOne)
                {
                    _minSize = base.Panel1MinSize;
                    base.Panel1MinSize = 0;
                    base.SplitterDistance = 0;
                }
                else
                {
                    int width = base.Orientation == Orientation.Horizontal ?
                        base.Height : base.Width;
                    _minSize = base.Panel2MinSize;
                    base.Panel2MinSize = 0;
                    base.SplitterDistance = width - base.SplitterWidth - base.Padding.Vertical;
                }
                base.Invalidate(base.SplitterRectangle);
            }
        }

        #endregion 缩起来

        #region 展开

        /// <summary>展开</summary>
        public void Expand()
        {
            if (_collapsePanel != CollapsePanel.None && _splitPanelState == SplitPanelState.Collapsed)
            {
                if (_collapsePanel == CollapsePanel.PanelOne)
                {
                    base.Panel1MinSize = _minSize;
                }
                else
                {
                    base.Panel2MinSize = _minSize;
                }
                base.SplitterDistance = _lastSplitDistance;
                base.Invalidate(base.SplitterRectangle);
            }
        }

        #endregion 展开

        #region 点击伸缩

        /// <summary>点击伸缩</summary>
        /// <param name="e"></param>
        protected virtual void OnCollapseClick(EventArgs e)
        {
            if (_splitPanelState == SplitPanelState.Collapsed)
            {
                SpliterPanelState = SplitPanelState.Expanded;
            }
            else
            {
                SpliterPanelState = SplitPanelState.Collapsed;
            }
            EventHandler handler = base.Events[eventCollapseClick] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion 点击伸缩

        #region 重绘控件

        /// <summary>重绘控件</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (base.Panel1Collapsed || base.Panel2Collapsed)
            {
                return;
            }
            Graphics g = e.Graphics;
            Rectangle rect = base.SplitterRectangle;
            bool bHorizontal = base.Orientation == Orientation.Horizontal;
            LinearGradientMode gradientMode = bHorizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal;
            using (LinearGradientBrush brush = new LinearGradientBrush(rect, _splitColor, Color.FromArgb(50, _splitColor), gradientMode))
            {
                //Blend blend = new Blend();
                //blend.Positions = new float[] { 0f, .5f, 1f };
                //blend.Factors = new float[] { .5F, 1F, .5F };
                //brush.Blend = blend;
                g.FillRectangle(brush, rect);
            }
            if (_collapsePanel == CollapsePanel.None)
            {
                return;
            }
            Rectangle arrowRect;
            Rectangle topLeftRect;
            Rectangle bottomRightRect;
            CalculateRect(CollapseRect, out arrowRect, out topLeftRect, out bottomRightRect);
            ArrowDirection direction = ArrowDirection.Left;
            switch (_collapsePanel)
            {
                case CollapsePanel.PanelOne:
                    if (bHorizontal)
                    {
                        direction = _splitPanelState == SplitPanelState.Collapsed ? ArrowDirection.Down : ArrowDirection.Up;
                    }
                    else
                    {
                        direction = _splitPanelState == SplitPanelState.Collapsed ? ArrowDirection.Right : ArrowDirection.Left;
                    }
                    break;

                case CollapsePanel.PanelTwo:
                    if (bHorizontal)
                    {
                        direction = _splitPanelState == SplitPanelState.Collapsed ? ArrowDirection.Up : ArrowDirection.Down;
                    }
                    else
                    {
                        direction = _splitPanelState == SplitPanelState.Collapsed ? ArrowDirection.Left : ArrowDirection.Right;
                    }
                    break;
            }
            //Color foreColor = _splitContainerState == SplitContainerState.Hover ? Color.FromArgb(21, 66, 139) : Color.FromArgb(80, 136, 228);
            Color foreColor = _arrowColor;// _splitContainerState == SplitContainerState.Hover ? Color.Red : Color.DeepPink;
            g.SmoothingMode = SmoothingMode.HighQuality;
            ControlRender.RenderRectangle(g, topLeftRect, new Size(3, 3), foreColor);
            ControlRender.RenderRectangle(g, bottomRightRect, new Size(3, 3), foreColor);
            using (Brush brush = new SolidBrush(foreColor))
            {
                ControlRender.RenderArrow(g, arrowRect, direction, brush);
            }
            DrawControlBorder(this.Panel1);
            DrawControlBorder(this.Panel2);
            DrawControlBorder(this);
        }

        #endregion 重绘控件

        #region 绘制Panel边框

        /// <summary>绘制Panel边框</summary>
        /// <param name="panel"></param>
        private void DrawControlBorder(Control ct)
        {
            if (ct is SplitterPanel)
            {
                ((SplitterPanel)ct).BorderStyle = BorderStyle.None;
                foreach (Control c in ct.Controls)
                {
                    c.Invalidate();
                }
            }
            if (ct is System.Windows.Forms.SplitContainer)
            {
                ((System.Windows.Forms.SplitContainer)ct).BorderStyle = BorderStyle.None;
            }
            Graphics graphics = ct.CreateGraphics();
            Pen pen = new Pen(_BorderColor, 1);
            Rectangle rectangle = ct.ClientRectangle;
            rectangle.X += 0;
            rectangle.Y += 0;
            rectangle.Width -= 1 * 2;
            rectangle.Height -= 1 * 2;
            graphics.DrawRectangle(pen, rectangle);
        }

        #endregion 绘制Panel边框

        #region 鼠标移动

        /// <summary>鼠标移动</summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //如果鼠标的左键没有按下，重置HistTest
            if (e.Button != MouseButtons.Left)
            {
                _splitButtonSatate = SplitButtonSatate.None;
            }
            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;
            //鼠标在Button矩形里，并且不是在拖动
            if (collapseRect.Contains(mousePoint) && _splitButtonSatate != SplitButtonSatate.Spliter)
            {
                base.Capture = false;
                SetCursor(Cursors.Hand);
                _splitContainerState = SplitContainerState.Hover;
                return;
            }//鼠标在分隔栏矩形里
            else if (base.SplitterRectangle.Contains(mousePoint))
            {
                _splitContainerState = SplitContainerState.Normal;
                //如果已经在按钮按下了鼠标或者已经收缩，就不允许拖动了
                if (_splitButtonSatate == SplitButtonSatate.Button || (_collapsePanel != CollapsePanel.None && _splitPanelState == SplitPanelState.Collapsed))
                {
                    base.Capture = false;
                    base.Cursor = Cursors.Default;
                    return;
                }
                //鼠标没有按下，设置Split光标
                if (_splitButtonSatate == SplitButtonSatate.None &&
                    !base.IsSplitterFixed)
                {
                    if (base.Orientation == Orientation.Horizontal)
                    {
                        SetCursor(Cursors.HSplit);
                    }
                    else
                    {
                        SetCursor(Cursors.VSplit);
                    }
                    return;
                }
            }
            _splitContainerState = SplitContainerState.Normal;
            //正在拖动分隔栏
            if (_splitButtonSatate == SplitButtonSatate.Spliter && !base.IsSplitterFixed)
            {
                if (base.Orientation == Orientation.Horizontal)
                {
                    SetCursor(Cursors.HSplit);
                }
                else
                {
                    SetCursor(Cursors.VSplit);
                }
                base.OnMouseMove(e);
                return;
            }
            base.Cursor = Cursors.Default;
            base.OnMouseMove(e);
        }

        #endregion 鼠标移动

        #region 鼠标离开

        /// <summary>鼠标离开</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.Cursor = Cursors.Default;
            _splitContainerState = SplitContainerState.Normal;
            base.OnMouseLeave(e);
        }

        #endregion 鼠标离开

        #region 鼠标按下

        /// <summary>鼠标按下</summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;
            if (collapseRect.Contains(mousePoint) || (_collapsePanel != CollapsePanel.None && _splitPanelState == SplitPanelState.Collapsed))
            {
                _splitButtonSatate = SplitButtonSatate.Button;
                return;
            }
            if (base.SplitterRectangle.Contains(mousePoint))
            {
                _splitButtonSatate = SplitButtonSatate.Spliter;
            }
            base.OnMouseDown(e);
        }

        #endregion 鼠标按下

        #region 键盘松下

        /// <summary>键盘松下</summary>
        /// <param name="e"></param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            base.Invalidate(base.SplitterRectangle);
        }

        #endregion 键盘松下

        #region 鼠标松下

        /// <summary>鼠标松下</summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            base.Invalidate(base.SplitterRectangle);
            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;
            if (_splitButtonSatate == SplitButtonSatate.Button && e.Button == MouseButtons.Left && collapseRect.Contains(mousePoint))
            {
                OnCollapseClick(EventArgs.Empty);
            }
            _splitButtonSatate = SplitButtonSatate.None;
        }

        #endregion 鼠标松下

        #region 设置鼠标光标

        /// <summary>设置鼠标光标</summary>
        /// <param name="cursor"></param>
        private void SetCursor(Cursor cursor)
        {
            if (base.Cursor != cursor)
            {
                base.Cursor = cursor;
            }
        }

        #endregion 设置鼠标光标

        #region 计算区域

        /// <summary>计算区域</summary>
        /// <param name="collapseRect"></param>
        /// <param name="arrowRect"></param>
        /// <param name="topLeftRect"></param>
        /// <param name="bottomRightRect"></param>
        private void CalculateRect(Rectangle collapseRect, out Rectangle arrowRect, out Rectangle topLeftRect, out Rectangle bottomRightRect)
        {
            int width;
            if (base.Orientation == Orientation.Horizontal)
            {
                width = (collapseRect.Width - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(collapseRect.X + width, collapseRect.Y, DefaultArrowWidth, collapseRect.Height);
                topLeftRect = new Rectangle(collapseRect.X, collapseRect.Y + 1, width, collapseRect.Height - 2);
                bottomRightRect = new Rectangle(arrowRect.Right, collapseRect.Y + 1, width, collapseRect.Height - 2);
            }
            else
            {
                width = (collapseRect.Height - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(collapseRect.X, collapseRect.Y + width, collapseRect.Width, DefaultArrowWidth);
                topLeftRect = new Rectangle(collapseRect.X + 1, collapseRect.Y, collapseRect.Width - 2, width);
                bottomRightRect = new Rectangle(collapseRect.X + 1, arrowRect.Bottom, collapseRect.Width - 2, width);
            }
        }

        #endregion 计算区域
    }

    #endregion 分割容器控件
}