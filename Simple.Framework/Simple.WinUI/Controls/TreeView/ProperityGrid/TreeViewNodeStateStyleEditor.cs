namespace Simple.WinUI.Controls.TreeView.ProperityGrid
{
    #region 树形节点选中状态样式编辑窗

    /// <summary>WWW.CSharpSkin.COM 树形节点选中状态样式编辑窗</summary>
    public partial class TreeViewNodeStateStyleEditor : Form
    {
        private TreeViewNodeStateStyle _treeViewNodeStateStyle;

        /// <summary>返回当前属性节点状态样式</summary>
        public TreeViewNodeStateStyle TreeViewNodeStateStyle
        {
            get
            {
                return _treeViewNodeStateStyle;
            }
        }

        #region 构造

        /// <summary>构造</summary>
        /// <param name="treeViewNodeStateStyle"></param>
        public TreeViewNodeStateStyleEditor(TreeViewNodeStateStyle treeViewNodeStateStyle)
        {
            InitializeComponent();
            this._treeViewNodeStateStyle = treeViewNodeStateStyle;
            this.Init();
            this.InitEvent();
        }

        #endregion 构造

        #region 初始化值

        /// <summary>初始化值</summary>
        private void Init()
        {
            this.plBrushColor.BackColor = _treeViewNodeStateStyle.SolidBrush.Color;
            this.plPenColor.BackColor = _treeViewNodeStateStyle.Pen.Color;
        }

        #endregion 初始化值

        #region 加载事件

        /// <summary>加载事件</summary>
        public void InitEvent()
        {
            foreach (Control ct in this.Controls)
            {
                if (ct is System.Windows.Forms.Panel)
                {
                    ct.Click += ColorPannel_Click;
                }
            }
        }

        #endregion 加载事件

        #region 选择颜色事件

        /// <summary>选择颜色事件</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorPannel_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                ((System.Windows.Forms.Panel)sender).BackColor = colorDialog.Color;
            }
        }

        #endregion 选择颜色事件

        #region 确定

        /// <summary>确定</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            _treeViewNodeStateStyle = new TreeViewNodeStateStyle();
            _treeViewNodeStateStyle.SolidBrush = new SolidBrush(this.plBrushColor.BackColor);
            _treeViewNodeStateStyle.Pen = new Pen(this.plPenColor.BackColor);
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion 确定
    }

    #endregion 树形节点选中状态样式编辑窗
}