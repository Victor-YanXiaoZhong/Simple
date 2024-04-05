using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls
{
    #region  复选框状态
    /// <summary>
    /// 复选框状态
    /// </summary>
    public enum CheckBoxState
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
