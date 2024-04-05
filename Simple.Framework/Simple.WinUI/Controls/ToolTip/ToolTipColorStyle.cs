using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls.ToolTip
{
    #region ToolTip颜色风格
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// ToolTip颜色风格
    /// </summary>
    public class ToolTipColorStyle
    {
        #region 成员
        /// <summary>
        /// 成员
        /// </summary>
        private Color _baseColor = Color.FromArgb(105, 200, 254);
        private Color _borderColor = Color.FromArgb(204, 153, 51);
        private Color _backNormalColor = Color.FromArgb(250, 250, 250);
        private Color _backHoverColor = Color.FromArgb(255, 180, 105);
        private Color _backPressedColor = Color.FromArgb(226, 176, 0);
        private Color _titleForeColor = Color.Brown;
        private Color _tipForeColor = Color.Chocolate;
        #endregion

        #region 基础颜色
        /// <summary>
        /// 基础颜色
        /// </summary>
        public Color BaseColor { get => _baseColor; set => _baseColor = value; }
        #endregion

        #region 边框颜色
        /// <summary>
        /// 边框颜色
        /// </summary>
        public Color BorderColor { get => _borderColor; set => _borderColor = value; }
        #endregion

        #region 正常背景颜色
        /// <summary>
        /// 正常背景颜色
        /// </summary>
        public Color BackNormalColor { get => _backNormalColor; set => _backNormalColor = value; }
        #endregion

        #region 鼠标进入背景颜色
        /// <summary>
        /// 鼠标进入背景颜色
        /// </summary>
        public Color BackHoverColor { get => _backHoverColor; set => _backHoverColor = value; }
        #endregion

        #region 鼠标按下背景颜色
        /// <summary>
        /// 鼠标按下背景颜色
        /// </summary>
        public Color BackPressedColor { get => _backPressedColor; set => _backPressedColor = value; }
        #endregion

        #region 标题颜色
        /// <summary>
        /// 标题颜色
        /// </summary>
        public Color TitleForeColor { get => _titleForeColor; set => _titleForeColor = value; }
        #endregion

        #region 内容颜色
        /// <summary>
        /// 内容颜色
        /// </summary>
        public Color TipForeColor { get => _tipForeColor; set => _tipForeColor = value; }
        #endregion
    }
    #endregion
}
