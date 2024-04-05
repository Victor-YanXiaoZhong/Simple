namespace Simple.WinUI.Controls.TrackBar.ProperityGrid
{
    #region 滑动条样式编辑框

    /// <summary>WWW.CSharpSkin.COM 滑动条样式编辑框</summary>
    public partial class TrackBarColorStyleEditor : Form
    {
        #region 滑动条样式

        /// <summary>滑动条样式</summary>
        private TrackBarColorStyle _trackBarColorStyle;

        /// <summary>返回滑动条样式</summary>
        public TrackBarColorStyle TrackBarColorStyle
        {
            get
            {
                return _trackBarColorStyle;
            }
        }

        #endregion 滑动条样式

        #region 构造

        /// <summary>构造</summary>
        /// <param name="trackBarColorStyle"></param>
        public TrackBarColorStyleEditor(TrackBarColorStyle trackBarColorStyle)
        {
            InitializeComponent();
            _trackBarColorStyle = trackBarColorStyle;
            Init();
            InitEvent();
        }

        #endregion 构造

        #region 初始化值

        /// <summary>初始化值</summary>
        private void Init()
        {
            this.plTrackBackgroundHoverColor.BackColor = _trackBarColorStyle.TrackBackgroundHoverColor;
            this.plTrackBackgroundNormalColor.BackColor = _trackBarColorStyle.TrackBackgroundNormalColor;
            this.plTrackBackgroundPressedColor.BackColor = _trackBarColorStyle.TrackBackgroundPressedColor;
            this.plTrackBeginColor.BackColor = _trackBarColorStyle.TrackBeginColor;
            this.plTrackBorderColor.BackColor = _trackBarColorStyle.TrackBorderColor;
            this.plTrackBorderHoverColor.BackColor = _trackBarColorStyle.TrackBorderHoverColor;
            this.plTrackBorderNormalColor.BackColor = _trackBarColorStyle.TrackBorderNormalColor;
            this.plTrackDarkColor.BackColor = _trackBarColorStyle.TrackDarkColor;
            this.plTrackEndColor.BackColor = _trackBarColorStyle.TrackEndColor;
            this.plTrackInnerBorderColor.BackColor = _trackBarColorStyle.TrackInnerBorderColor;
            this.plTrackLightColor.BackColor = _trackBarColorStyle.TrackLightColor;
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

        #region 确定样式

        /// <summary>确定</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this._trackBarColorStyle.TrackBackgroundHoverColor = Color.FromArgb(200, this.plTrackBackgroundHoverColor.BackColor);
            this._trackBarColorStyle.TrackBackgroundNormalColor = plTrackBackgroundNormalColor.BackColor;
            this._trackBarColorStyle.TrackBackgroundPressedColor = Color.FromArgb(200, plTrackBackgroundPressedColor.BackColor);
            this._trackBarColorStyle.TrackBeginColor = plTrackBeginColor.BackColor;
            this._trackBarColorStyle.TrackBorderColor = plTrackBorderColor.BackColor;
            this._trackBarColorStyle.TrackBorderHoverColor = plTrackBorderHoverColor.BackColor;
            this._trackBarColorStyle.TrackBorderNormalColor = plTrackBorderNormalColor.BackColor;
            this._trackBarColorStyle.TrackDarkColor = plTrackDarkColor.BackColor;
            this._trackBarColorStyle.TrackEndColor = plTrackEndColor.BackColor;
            this._trackBarColorStyle.TrackInnerBorderColor = Color.FromArgb(200, plTrackInnerBorderColor.BackColor);
            this._trackBarColorStyle.TrackLightColor = plTrackLightColor.BackColor;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion 确定样式
    }

    #endregion 滑动条样式编辑框
}