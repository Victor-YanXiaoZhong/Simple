using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls.TreeView
{
    #region  树形节点样式
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 树形节点样式
    /// </summary>

    public class TreeViewNodeStateStyle
    {
        #region 节点背景填充颜色
        /// <summary>
        /// 节点背景填充颜色
        /// </summary>
        private SolidBrush _solidBrush = new SolidBrush(Color.FromArgb(209, 232, 255));//填充颜色
        [Browsable(true)]
        [Description("节点背景填充颜色")]
        public SolidBrush SolidBrush
        {
            get
            {
                return _solidBrush;
            }
            set
            {
                _solidBrush = value;
            }
        }
        #endregion

        #region 节点边框颜色
        /// <summary>
        /// 节点边框颜色
        /// </summary>
        private Pen _pen = new Pen(Color.FromArgb(102, 167, 232), 1);//边框颜色
        [Browsable(true)]
        [Description("节点边框颜色")]
        public Pen Pen
        {
            get { return _pen; }
            set
            {
                _pen = value;
            }
        }
        #endregion
    }
    #endregion
}
