using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Simple.WinUI.Controls.TreeView.ProperityGrid;

namespace Simple.WinUI.Controls.TreeView
{
    #region 带有样式的属性控件

    /// <summary>WWW.CSharpSkin.COM 带有样式的属性控件</summary>
    public class UTreeView : System.Windows.Forms.TreeView
    {
        #region 定义常量

        /// <summary>定义常量</summary>
        private const int WM_VSCROLL = 0x0115;

        private const int WM_HSCROLL = 0x0114;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int TV_FIRST = 0x1100;
        private const int TVM_SETBKCOLOR = TV_FIRST + 29;
        private const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;

        #endregion 定义常量

        #region 构造

        /// <summary>构造</summary>
        public UTreeView()
        {
            //首先开启双缓冲，防止闪烁
            //双缓冲的设置 具体参数含义参照msdn的ControlStyles枚举值
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.ItemHeight = 30;
            this.BackColor = Color.Transparent;
        }

        #endregion 构造

        #region 定义事件委托

        /// <summary>定义事件委托</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventHandler(object sender, EventArgs e);

        #endregion 定义事件委托

        #region 定义滚动时触发事件

        /// <summary>滚动时触发</summary>
        public event EventHandler Scroll;

        #endregion 定义滚动时触发事件

        #region 控件滚动时触发

        /// <summary>控件滚动时触发</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnScroll(object sender, EventArgs e)
        {
            if (Scroll != null)
                Scroll(sender, e);
        }

        #endregion 控件滚动时触发

        #region 更新展开节点样式

        /// <summary>更新展开节点样式</summary>
        private void UpdateExtendedStyles()
        {
            int Style = 0;

            if (DoubleBuffered)
                Style |= TVS_EX_DOUBLEBUFFER;

            if (Style != 0)
                WinApi.SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)Style);
        }

        #endregion 更新展开节点样式

        #region 重写句柄创建

        /// <summary>重写句柄创建事件</summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateExtendedStyles();
            if (!WinApi.IsXPOS)
                WinApi.SendMessage(Handle, TVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(BackColor));
        }

        #endregion 重写句柄创建

        #region 重写控件

        /// <summary>重写控件</summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint))
            {
                Message m = new Message();
                m.HWnd = Handle;
                m.Msg = WinApi.WM_PRINTCLIENT;
                m.WParam = e.Graphics.GetHdc();
                m.LParam = (IntPtr)WinApi.PRF_CLIENT;
                DefWndProc(ref m);
                e.Graphics.ReleaseHdc(m.WParam);
            }
            base.OnPaint(e);
        }

        #endregion 重写控件

        #region 接收操作系统发送过来的消息

        /// <summary>接收操作系统发送过来的消息</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (WinApi.IsHorizontalScrollBarVisible(this))
                WinApi.ShowScrollBar(this.Handle, 0, false);

            if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL || m.Msg == WM_MOUSEWHEEL)
                OnScroll(new object(), new EventArgs());
            base.WndProc(ref m);
        }

        #endregion 接收操作系统发送过来的消息

        #region 节点被选中 ,TreeView有焦点时节点样式

        /// <summary>节点被选中 ,TreeView有焦点</summary>
        private TreeViewNodeStateStyle _selectFocusNodeStateStyle = new TreeViewNodeStateStyle() { Pen = new Pen(Color.FromArgb(102, 167, 232), 1), SolidBrush = new SolidBrush(Color.FromArgb(209, 232, 255)) };

        [Description("节点被选中 ,TreeView有焦点时节点样式")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(TreeViewNodeStateStylePropertyEditor), typeof(UITypeEditor))]
        public TreeViewNodeStateStyle SelectFocusNodeStateStyle
        {
            get
            {
                if (_selectFocusNodeStateStyle == null)
                {
                    _selectFocusNodeStateStyle = new TreeViewNodeStateStyle();
                }
                return _selectFocusNodeStateStyle;
            }
            set
            {
                _selectFocusNodeStateStyle = value;
                this.Invalidate();
            }
        }

        #endregion 节点被选中 ,TreeView有焦点时节点样式

        #region 节点被选中，TreeView无焦点

        /// <summary>节点被选中，TreeView无焦点</summary>
        private TreeViewNodeStateStyle _selectNodeStateStyle = new TreeViewNodeStateStyle() { Pen = new Pen(Color.FromArgb(222, 222, 222), 1), SolidBrush = new SolidBrush(Color.FromArgb(247, 247, 247)) };

        [Browsable(true)]
        [Description("节点被选中 ,TreeViewTreeView无焦点节点样式")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(TreeViewNodeStateStylePropertyEditor), typeof(UITypeEditor))]
        public TreeViewNodeStateStyle SelectNodeStateStyle
        {
            get
            {
                if (_selectNodeStateStyle == null)
                {
                    _selectNodeStateStyle = new TreeViewNodeStateStyle();
                }
                return _selectNodeStateStyle;
            }
            set
            {
                _selectNodeStateStyle = value;
                this.Invalidate();
            }
        }

        #endregion 节点被选中，TreeView无焦点

        #region 鼠标移动到节点上的节点样式

        /// <summary>鼠标移动到节点上的节点样式</summary>
        private TreeViewNodeStateStyle _moveNodeStateStyle = new TreeViewNodeStateStyle() { Pen = new Pen(Color.FromArgb(112, 192, 231), 1), SolidBrush = new SolidBrush(Color.FromArgb(229, 243, 251)) };

        [Browsable(true)]
        [Description("鼠标移动到节点上的节点样式")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(TreeViewNodeStateStylePropertyEditor), typeof(UITypeEditor))]
        public TreeViewNodeStateStyle MoveNodeStateStyle
        {
            get
            {
                if (_moveNodeStateStyle == null)
                {
                    _moveNodeStateStyle = new TreeViewNodeStateStyle();
                }
                return _moveNodeStateStyle;
            }
            set
            {
                _moveNodeStateStyle = value;
                this.Invalidate();
            }
        }

        #endregion 鼠标移动到节点上的节点样式

        #region 节点扩展开的时候的图标

        /// <summary>需要缩起节点的图标</summary>
        private Image _expandIcon;

        [Browsable(true)]
        [Description("需要缩起节点的图标")]
        public Image ExpandIcon
        {
            get { return _expandIcon; }
            set
            {
                _expandIcon = value;
                this.Invalidate();
            }
        }

        #endregion 节点扩展开的时候的图标

        #region 节点缩起来的图标

        /// <summary>需要展开节点的图标</summary>
        private Image _shrinkIcon;

        [Browsable(true)]
        [Description("节点缩起来的图标")]
        public Image ShrinkIcon
        {
            get { return _shrinkIcon; }
            set
            {
                _shrinkIcon = value;
                this.Invalidate();
            }
        }

        #endregion 节点缩起来的图标

        #region 绘制节点

        private Point currentLocation = Point.Empty;
        private TreeNode currentNode = null;

        /// <summary>绘制节点</summary>
        /// <param name="e"></param>
        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            base.OnDrawNode(e);

            #region 选中的节点背景

            Rectangle nodeRect = new Rectangle(1, e.Bounds.Top, e.Bounds.Width - 3, e.Bounds.Height - 1);
            if (e.Node.IsSelected)
            {
                //TreeView有焦点的时候 画选中的节点
                if (this.Focused)
                {
                    e.Graphics.FillRectangle(_selectFocusNodeStateStyle.SolidBrush, nodeRect);
                    e.Graphics.DrawRectangle(_selectFocusNodeStateStyle.Pen, nodeRect);
                }
                //TreeView失去焦点的时候
                else
                {
                    e.Graphics.FillRectangle(_selectNodeStateStyle.SolidBrush, nodeRect);
                    e.Graphics.DrawRectangle(_selectNodeStateStyle.Pen, nodeRect);
                }
            }
            else if ((e.State & TreeNodeStates.Hot) != 0 && e.Node.Text != "")//|| currentMouseMoveNode == e.Node)
            {
                e.Graphics.FillRectangle(_moveNodeStateStyle.SolidBrush, nodeRect);
                e.Graphics.DrawRectangle(_moveNodeStateStyle.Pen, nodeRect);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), e.Bounds);
            }

            #endregion 选中的节点背景

            #region 扩展和缩小图标绘制

            Rectangle plusRect = new Rectangle(e.Node.Bounds.Left - 20, nodeRect.Top + 9, 20, 20);
            if (e.Node.IsExpanded && e.Node.Nodes.Count > 0)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                    if (_expandIcon != null) e.Graphics.DrawImage(_expandIcon, plusRect);
            }
            else if (e.Node.IsExpanded == false && e.Node.Nodes.Count > 0)
            {
                for (int i = 0; i < this.Nodes.Count; i++)
                    if (_shrinkIcon != null) e.Graphics.DrawImage(_shrinkIcon, plusRect);
            }

            TreeViewHitTestInfo info = this.HitTest(currentLocation);
            if (currentNode != null && info.Location == TreeViewHitTestLocations.PlusMinus && currentNode == e.Node)
            {
                if (currentNode.IsExpanded && currentNode.Nodes.Count > 0)
                    if (_expandIcon != null) e.Graphics.DrawImage(_expandIcon, plusRect);
                    else if (currentNode.IsExpanded == false && currentNode.Nodes.Count > 0)
                        if (_shrinkIcon != null) e.Graphics.DrawImage(_shrinkIcon, plusRect);
            }

            #endregion 扩展和缩小图标绘制

            #region 画节点文本

            Rectangle nodeTextRect = new Rectangle(e.Node.Bounds.Left + 2 + 18, e.Node.Bounds.Top + 12, e.Node.Bounds.Width + 2, e.Node.Bounds.Height);
            nodeTextRect.Width += 4;
            nodeTextRect.Height -= 4;
            e.Graphics.DrawString(e.Node.Text, e.Node.TreeView.Font, new SolidBrush(this.ForeColor), nodeTextRect);

            #endregion 画节点文本

            #region 画ImageList中的图片

            int currt_X = e.Node.Bounds.X;
            if (this.ImageList != null && this.ImageList.Images.Count > 0)
            {
                //图标大小18*18
                Rectangle imagebox = new Rectangle(
                    e.Node.Bounds.X - 3 - ImageList.ImageSize.Width,
                    e.Node.Bounds.Y + 3,
                    ImageList.ImageSize.Width,
                    ImageList.ImageSize.Height);
                imagebox = new Rectangle(
                    e.Node.Bounds.Left + 2,
                    e.Node.Bounds.Top + 10,
                    ImageList.ImageSize.Width,
                    ImageList.ImageSize.Height);
                int index = e.Node.ImageIndex;
                string imagekey = e.Node.ImageKey;
                if (imagekey != "" && this.ImageList.Images.ContainsKey(imagekey))
                    e.Graphics.DrawImage(this.ImageList.Images[imagekey], imagebox);
                else
                {
                    if (e.Node.ImageIndex < 0)
                        index = 0;
                    else if (index > this.ImageList.Images.Count - 1)
                        index = 0;
                    e.Graphics.DrawImage(this.ImageList.Images[index], imagebox);
                }
            }

            #endregion 画ImageList中的图片
        }

        #endregion 绘制节点

        #region 如果点击相对应的地方，获取相关的节点并选中

        /// <summary>如果点击相对应的地方，获取相关的节点并选中</summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            TreeNode treeNode = this.GetNodeAt(e.Location);
            if (treeNode != null)
            {
                this.SelectedNode = treeNode;
            }
        }

        #endregion 如果点击相对应的地方，获取相关的节点并选中
    }

    #endregion 带有样式的属性控件
}