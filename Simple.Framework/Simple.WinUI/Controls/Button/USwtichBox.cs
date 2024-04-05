using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Simple.WinUI.Controls.Button
{
    #region 切换卡图片按钮

    /// <summary>切换卡图片按钮</summary>
    public class USwtichBox : System.Windows.Forms.PictureBox
    {
        #region 构造

        /// <summary>构造</summary>
        public USwtichBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        #endregion 构造

        #region 定义事件，值改变时发生

        public event ValueChanged OnValueChanged;

        /// <summary>定义事件，值改变时发生</summary>
        /// <param name="value"></param>
        public delegate void ValueChanged(int value);

        #endregion 定义事件，值改变时发生

        #region 切换后的颜色

        /// <summary>切换后的颜色</summary>
        private Color _swichColor = Color.PaleGreen;

        [Browsable(true)]
        [Description("切换后的颜色")]
        public Color DefaultSwichColor
        {
            get { return _swichColor; }
            set
            {
                _swichColor = value;
                if (_swichColor != null)
                {
                    //_swichColor.MakeTransparent(Color.FromArgb(192, 192, 192));
                    Invalidate();
                }
            }
        }

        #endregion 切换后的颜色

        #region 未切换的颜色(灰度)

        /// <summary>未切换的颜色(灰度)</summary>
        private Color _switchDefault = Color.WhiteSmoke;

        [Browsable(true)]
        [Description("未切换的颜色(灰度)")]
        public Color SwitchDefault
        {
            get { return _switchDefault; }
            set
            {
                _switchDefault = value;
                if (_switchDefault != null)
                {
                    Invalidate();
                }
            }
        }

        #endregion 未切换的颜色(灰度)

        #region 值

        /// <summary>值</summary>
        private int _value = 0;

        [Browsable(true)]
        [Description("是否默认,0为默认 1为切换")]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value > 1) value = 1;
                if (value < 1) value = 0;
                _value = value;
                if (OnValueChanged != null)
                {
                    OnValueChanged.Invoke(value);
                }
                Invalidate();
            }
        }

        #endregion 值

        #region 值描述

        /// <summary>值</summary>
        private string[] _decValue = new string[2] { "开", "关" };

        [Browsable(true)]
        [Description("描述显示,1行为默认 2行为切换")]
        public string[] DecValue
        {
            get { return _decValue; }
            set
            {
                _decValue = value;
                if (value.Length < 2)
                {
                }
                Invalidate();
            }
        }

        #endregion 值描述

        #region 鼠标单击

        /// <summary>鼠标单击</summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (Value == 1)
            {
                Value = 0;
            }
            else
            {
                Value = 1;
            }
            Invalidate();
        }

        #endregion 鼠标单击

        #region 控件尺寸改变触发事件

        /// <summary>控件尺寸改变触发事件</summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Invalidate();
        }

        #endregion 控件尺寸改变触发事件

        #region 绘制控件

        /// <summary>绘制控件</summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            var preColor = Value == 1 ? _switchDefault : _swichColor;
            var endColor = Value == 0 ? _switchDefault : _swichColor;
            //前部分
            ControlRender.RenderBackground(pe.Graphics,
                new Rectangle(0, 0, Width / 2, Height),
                preColor, preColor, preColor, RoundStyle.All, true, true, LinearGradientMode.Vertical);
            //后部分
            ControlRender.RenderBackground(pe.Graphics,
                new Rectangle(Width / 2, 0, Width / 2, Height),
                endColor, endColor, endColor, RoundStyle.All, 50, 0.45f, true, true, LinearGradientMode.Vertical);
            pe.Dispose();
        }

        #endregion 绘制控件
    }

    #endregion 切换卡图片按钮
}