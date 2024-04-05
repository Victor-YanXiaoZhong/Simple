using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls
{
    #region 鼠标状态

    /// <summary>WWW.CSharpSkin.COM 鼠标状态</summary>
    public class MouseState
    {
        #region 是否按下左键

        /// <summary>是否按下左键</summary>
        /// <returns></returns>
        public static bool IsMouseLeftKeyPress()
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                return (WinApi.GetKeyState(WinApi.RBUTTON) < 0);
            }
            else
            {
                return (WinApi.GetKeyState(WinApi.LBUTTON) < 0);
            }
        }

        #endregion 是否按下左键
    }

    #endregion 鼠标状态
}