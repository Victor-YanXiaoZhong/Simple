using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls.TrackBar
{
    #region 滑动条颜色样式
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 滑动条颜色样式
    /// </summary>
    [Serializable]
    public class TrackBarColorStyle
    {
        #region 滑动条背景开始颜色
        private Color _trackBeginColor = Color.FromArgb(120, 120, 120);
        /// <summary>
        /// 滑动条背景开始颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条背景开始颜色")]
        public Color TrackBeginColor
        {
            get
            {
                return _trackBeginColor;
            }
            set
            {
                _trackBeginColor = value;
            }
        }
        #endregion

        #region 滑动条背景结束颜色
        private Color _trackEndColor = Color.FromArgb(255, 255, 255);
        /// <summary>
        /// 滑动条背景结束颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条背景结束颜色")]
        public Color TrackEndColor
        {
            get
            {
                return _trackEndColor;
            }
            set
            {
                _trackEndColor = value;
            }
        }
        #endregion

        #region 滑动条边框颜色
        private Color _trackBorderColor = Color.FromArgb(109, 109, 109);
        /// <summary>
        /// 滑动条边框颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条边框颜色")]
        public Color TrackBorderColor
        {
            get
            {
                return _trackBorderColor;
            }
            set
            {
                _trackBorderColor = value;
            }
        }
        #endregion

        #region 滑动条边框内部颜色
        private Color _trackInnerBorderColor = Color.FromArgb(200, 250, 250, 250);
        /// <summary>
        /// 滑动条边框内部颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条边框内部颜色")]
        public Color TrackInnerBorderColor
        {
            get
            {
                return _trackInnerBorderColor;
            }
            set
            {
                _trackInnerBorderColor = value;
            }
        }
        #endregion

        #region 滑块背景正常状态颜色
        private Color _trackBackgroundNormalColor = Color.FromArgb(200, 193, 227, 247);
        /// <summary>
        /// 滑块背景正常状态颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑块背景正常状态颜色")]
        public Color TrackBackgroundNormalColor
        {
            get
            {
                return _trackBackgroundNormalColor;
            }
            set
            {
                _trackBackgroundNormalColor = value;
            }
        }
        #endregion

        #region 滑动条背景鼠标进入颜色
        private Color _trackBackgroundHoverColor = Color.FromArgb(200, 50, 162, 228);
        /// <summary>
        /// 滑动条背景鼠标进入颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条背景鼠标进入颜色")]
        public Color TrackBackgroundHoverColor
        {
            get
            {
                return _trackBackgroundHoverColor;
            }
            set
            {
                _trackBackgroundHoverColor = value;
            }
        }
        #endregion

        #region 滑动条背景鼠标按下颜色
        /// <summary>
        /// 滑动条背景鼠标按下颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑动条背景鼠标按下颜色")]
        private Color _trackBackgroundPressedColor = Color.FromArgb(200, 50, 162, 228);
        public Color TrackBackgroundPressedColor
        {
            get
            {
                return _trackBackgroundPressedColor;
            }
            set
            {
                _trackBackgroundPressedColor = value;
            }
        }
        #endregion

        #region 滑块边框正常颜色
        private Color _trackBorderNormalColor = Color.FromArgb(103, 165, 216);
        /// <summary>
        /// 滑块边框正常颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑块边框正常颜色")]
        public Color TrackBorderNormalColor
        {
            get
            {
                return _trackBorderNormalColor;
            }
            set
            {
                _trackBorderNormalColor = value;
            }
        }
        #endregion

        #region 滑块边框鼠标进入颜色
        private Color _trackBorderHoverColor = Color.FromArgb(70, 146, 207);
        /// <summary>
        /// 滑块边框鼠标进入颜色
        /// </summary>
        [Browsable(true)]
        [Description("滑块边框鼠标进入颜色")]
        public Color TrackBorderHoverColor
        {
            get
            {
                return _trackBorderHoverColor;
            }
            set
            {
                _trackBorderHoverColor = value;
            }
        }
        #endregion

        #region 刻度颜色，刻度颜色有两种颜色组成
        //刻度颜色，刻度颜色有两种颜色组成
        #region 刻度浅颜色
        private Color _trackLightColor = Color.FromArgb(233, 238, 238);
        /// <summary>
        /// 刻度浅颜色
        /// </summary>
        [Browsable(true)]
        [Description("刻度浅颜色")]
        public Color TrackLightColor
        {
            get
            {
                return _trackLightColor;
            }
            set
            {
                _trackLightColor = value;
            }
        }
        #endregion

        #region 刻度深颜色
        private Color _trackDarkColor = Color.FromArgb(197, 197, 197);
        /// <summary>
        /// 刻度深颜色
        /// </summary>
        [Browsable(true)]
        [Description("刻度深颜色")]
        public Color TrackDarkColor
        {
            get
            {
                return _trackDarkColor;
            }
            set
            {
                _trackDarkColor = value;
            }
        }
        #endregion
        #endregion
    }
    #endregion
}
