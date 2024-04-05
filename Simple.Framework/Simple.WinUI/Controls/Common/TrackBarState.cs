using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls
{
    #region 滑动条状态
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 滑动条状态
    /// </summary>
    public enum TrackBarState
    {
        /// <summary>
        ///  正常。
        /// </summary>
        Normal,
        /// <summary>
        /// 鼠标进入。
        /// </summary>
        Hover,
        /// <summary>
        /// 鼠标按下。
        /// </summary>
        Pressed,
        /// <summary>
        /// 获得焦点。
        /// </summary>
        Focused
    }
    #endregion
}
