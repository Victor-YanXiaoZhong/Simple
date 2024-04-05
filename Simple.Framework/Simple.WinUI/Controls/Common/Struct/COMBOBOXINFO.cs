using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls
{
    #region 系统下拉框信息
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 系统下拉框信息
    /// </summary>
    public struct COMBOBOXINFO
    {
        public int cbSize;
        public RECT rcItem;
        public RECT rcButton;
        public COMBOBOXBUTTONSTATE _comBoboxButtonState;
        public IntPtr hwndCombo;
        public IntPtr hwndEdit;
        public IntPtr hwndList;
    }
    #endregion
}
