namespace Simple.WinUI.Controls.ToolTip.ProperityGrid
{
    #region CSharpToolTip颜色样式编辑器

    /// <summary>WWW.CSharpSkin.COM CSharpToolTip颜色样式编辑器</summary>
    public partial class ToolTipColorStyleEditor : Form
    {
        private ToolTipColorStyle _toolTipColorStyle;

        #region 返回CSharpToolTip颜色样式

        /// <summary>返回CSharpToolTip颜色样式</summary>
        public ToolTipColorStyle ToolTipColorStyle
        {
            get
            {
                return _toolTipColorStyle;
            }
        }

        #endregion 返回CSharpToolTip颜色样式

        #region 无参构造

        /// <summary>构造</summary>
        public ToolTipColorStyleEditor()
        {
            InitializeComponent();
        }

        #endregion 无参构造

        #region 有参构造

        /// <summary>有参构造</summary>
        /// <param name="stripColorStyle"></param>
        public ToolTipColorStyleEditor(ToolTipColorStyle toolTipColorStyle)
        {
            _toolTipColorStyle = toolTipColorStyle;
            InitializeComponent();
            this.Init();
            this.InitEvent();
        }

        #endregion 有参构造

        #region 初始化值

        /// <summary>初始化值</summary>
        private void Init()
        {
            this.plBackHoverColor.BackColor = _toolTipColorStyle.BackHoverColor;
            this.plBackNormalColor.BackColor = _toolTipColorStyle.BackNormalColor;
            this.plBackPressedColor.BackColor = _toolTipColorStyle.BackPressedColor;
            this.plBaseColor.BackColor = _toolTipColorStyle.BaseColor;
            this.plBorderColor.BackColor = _toolTipColorStyle.BorderColor;
            this.plTipForeColor.BackColor = _toolTipColorStyle.TipForeColor;
            this.plTitleForeColor.BackColor = _toolTipColorStyle.TitleForeColor;
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
        private void BtnOK_Click(object sender, EventArgs e)
        {
            this._toolTipColorStyle.BackHoverColor = this.plBackHoverColor.BackColor;
            this._toolTipColorStyle.BackNormalColor = this.plBackNormalColor.BackColor;
            this._toolTipColorStyle.BackPressedColor = this.plBackPressedColor.BackColor;
            this._toolTipColorStyle.BaseColor = this.plBaseColor.BackColor;
            this._toolTipColorStyle.BorderColor = this.plBorderColor.BackColor;
            this._toolTipColorStyle.TipForeColor = this.plTipForeColor.BackColor;
            this._toolTipColorStyle.TitleForeColor = this.plTitleForeColor.BackColor;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion 确定
    }

    #endregion CSharpToolTip颜色样式编辑器
}