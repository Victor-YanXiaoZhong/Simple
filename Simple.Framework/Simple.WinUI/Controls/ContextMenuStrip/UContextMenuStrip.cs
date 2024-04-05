using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.ContextMenuStrip
{
    #region 下拉菜单

    /// <summary>WWW.CSharpSkin.COM 下拉菜单</summary>
    public class UContextMenuStrip : System.Windows.Forms.ContextMenuStrip
    {
        #region 是否开启显示动画

        /// <summary>是否开启显示动画</summary>
        private bool _isOpenAnimation = true;

        [Browsable(true)]
        [Description("是否开启显示动画")]
        public bool IsOpenAnimation
        {
            get
            {
                return _isOpenAnimation;
            }
            set
            {
                _isOpenAnimation = value;
            }
        }

        #endregion 是否开启显示动画

        #region 设置菜单项的选中颜色

        /// <summary>设置菜单项的选中颜色</summary>
        private Color _currentColor = Color.Silver;

        [Browsable(true)]
        [Description("设置菜单项的选中颜色")]
        public Color CurrentColor
        {
            get { return _currentColor; }
            set
            {
                _currentColor = value;
                this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                ToolStripProfessionRenderer pf = new ToolStripProfessionRenderer(this._currentColor, this._currentOpacity, this.BackColor);
                this.Renderer = pf;
            }
        }

        #endregion 设置菜单项的选中颜色

        #region 设置菜单项的透明度

        /// <summary>设置菜单项的透明度</summary>
        private int _currentOpacity = 100;

        [Browsable(true)]
        [Description("设置菜单项的透明度,默认100")]
        public int CurrentOpacity
        {
            get { return _currentOpacity; }
            set
            {
                _currentOpacity = value;
                this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
                ToolStripProfessionRenderer pf = new ToolStripProfessionRenderer(this._currentColor, this._currentOpacity, this.BackColor);
                this.Renderer = pf;
            }
        }

        #endregion 设置菜单项的透明度

        #region 构造

        /// <summary>构造</summary>
        public UContextMenuStrip()
        {
            this.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            ToolStripProfessionRenderer pf = new ToolStripProfessionRenderer(this._currentColor, this._currentOpacity, this.BackColor);
            this.Renderer = pf;
        }

        #endregion 构造

        #region 打开时触发事件

        /// <summary>打开时触发事件</summary>
        /// <param name="e"></param>
        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            base.OnOpening(e);
            if (_isOpenAnimation)
            {
                WinApi.WindowsShow(this.Handle);
            }
        }

        public static implicit operator MenuStrip(UContextMenuStrip v)
        {
            throw new NotImplementedException();
        }

        #endregion 打开时触发事件
    }

    #endregion 下拉菜单
}