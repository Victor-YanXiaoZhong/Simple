using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.NumericUpDown
{
    #region 数字选择器绘制事件
    /// <summary>
    /// WWW.CSharpSkin.COM
    /// 数字选择器绘制事件
    /// </summary>
    public class NumericUpDownButtonPaintEventArgs : PaintEventArgs
    {
        #region 鼠标进入
        /// <summary>
        /// 鼠标进入
        /// </summary>
        private bool _mouseOver;
        public bool MouseOver
        {
            get { return _mouseOver; }
        }
        #endregion

        #region 鼠标按下
        /// <summary>
        /// 鼠标按下
        /// </summary>
        private bool _mousePress;
        public bool MousePress
        {
            get { return _mousePress; }
        }
        #endregion

        #region 鼠标进入上下按钮
        /// <summary>
        /// 鼠标进入上下按钮
        /// </summary>
        private bool _mouseInUpButton;
        public bool MouseInUpButton
        {
            get { return _mouseInUpButton; }
        }
        #endregion

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="clipRect"></param>
        /// <param name="mouseOver"></param>
        /// <param name="mousePress"></param>
        /// <param name="mouseInUpButton"></param>
        public NumericUpDownButtonPaintEventArgs(Graphics graphics, Rectangle clipRect, bool mouseOver, bool mousePress, bool mouseInUpButton) : base(graphics, clipRect)
        {
            _mouseOver = mouseOver;
            _mousePress = mousePress;
            _mouseInUpButton = mouseInUpButton;
        }
        #endregion
    }
    #endregion
}
