using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls
{
    #region 容器Pannel状态
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 容器Pannel状态
    /// </summary>
    public enum SplitPanelState
    {
        Collapsed = 0,
        Expanded = 1,
    }
    #endregion

    #region 分割容器控件状态
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 分割容器控件状态
    /// </summary>
    public enum SplitContainerState
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
        Focused,
    }
    #endregion

    #region 分割容器箭头
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 分割容器箭头
    /// </summary>
    public enum SplitArrowDirection
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 3,
        Down = 4,
        LeftRight = 5,
        UpDown = 6
    }
    #endregion

    #region 点击收缩按钮时隐藏的Panel
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 点击收缩按钮时隐藏的Panel
    /// </summary>
    public enum CollapsePanel
    {
        None = 0,
        PanelOne = 1,
        PanelTwo = 2,
    }
    #endregion

    #region 分割按钮状态
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 分割按钮状态
    /// </summary>
    public enum SplitButtonSatate
    {
        None,
        Button,
        Spliter
    }
    #endregion
}
