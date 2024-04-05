using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Controls.Button
{
    #region 按钮

    /// <summary>按钮</summary>
    public class UImageButton : System.Windows.Forms.Button
    {
        /// <summary>背景图片分割后的图片数组 1--表示分割为1行 4--表示每一行有1份 横向分割的话，[1,4]表示有1行每一行有4个 默认横向分割 纵向分割的话，[4,1]表示有4行每一行有1个</summary>
        private Bitmap[,] BackImageSplitArray = new Bitmap[1, 4];

        #region 横轴背景图片分割的份数,用于设置不同状态显示不同背景图片 默认为4

        /// <summary>横轴背景图片分割的份数,用于设置不同状态显示不同背景图片 默认为4</summary>
        private int _xBackImageSplitCount = 4;

        [Browsable(true)]
        [Description("横轴背景图片分割的份数,用于设置不同状态显示不同背景图片,默认4")]
        public int XBackImageSplitCount
        {
            get
            {
                if (_xBackImageSplitCount == 0)
                {
                    _xBackImageSplitCount = 4;
                }
                return _xBackImageSplitCount;
            }
            set
            {
                _xBackImageSplitCount = value;
                if (_csharpBackgroundImage != null)
                {
                    BackImageSplitArray = null;
                    BackImageSplitArray = BitmapCapture.Capture((Bitmap)this._csharpBackgroundImage, _xBackImageSplitCount, _yBackImageSplitCount);
                    this.Invalidate();
                }
            }
        }

        #endregion 横轴背景图片分割的份数,用于设置不同状态显示不同背景图片 默认为4

        #region 纵轴背景图片分割的份数,用于设置不同状态显示不同背景图片,默认1

        /// <summary>纵轴背景图片分割的份数,用于设置不同状态显示不同背景图片,默认1</summary>
        private int _yBackImageSplitCount = 1;

        [Browsable(true)]
        [Description("纵轴背景图片分割的份数,用于设置不同状态显示不同背景图片,默认1")]
        public int YBackImageSplitCount
        {
            get
            {
                if (_yBackImageSplitCount == 0)
                {
                    _yBackImageSplitCount = 1;
                }
                return _yBackImageSplitCount;
            }
            set
            {
                _yBackImageSplitCount = value;
                if (_csharpBackgroundImage != null)
                {
                    BackImageSplitArray = null;
                    BackImageSplitArray = BitmapCapture.Capture((Bitmap)this._csharpBackgroundImage, _xBackImageSplitCount, _yBackImageSplitCount);
                    this.Invalidate();
                }
            }
        }

        #endregion 纵轴背景图片分割的份数,用于设置不同状态显示不同背景图片,默认1

        #region 按钮背景图片

        /// <summary>按钮背景图片</summary>
        private Image _csharpBackgroundImage;

        [Browsable(true)]
        [Description("设置按钮图片,需要保证横向或纵向其中任一方向都可以被分割成一行四列或四行一列,四列或四行分别对应装为正常、鼠标按下去、鼠标移动上去、获取焦点")]
        public Image CsharpBackgroundImage
        {
            get { return _csharpBackgroundImage; }
            set
            {
                _csharpBackgroundImage = value;
                if (_csharpBackgroundImage != null)
                {
                    BackImageSplitArray = null;
                    BackImageSplitArray = BitmapCapture.Capture((Bitmap)this._csharpBackgroundImage, _xBackImageSplitCount, _yBackImageSplitCount);
                }
                this.Invalidate();
            }
        }

        #endregion 按钮背景图片

        #region 构造

        /// <summary>构造</summary>
        public UImageButton()
        {
            //首先开启双缓冲，防止闪烁
            //双缓冲的设置 具体参数含义参照msdn的ControlStyles枚举值
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            //初始化按钮背景图片
            if (_csharpBackgroundImage != null)
            {
                BitmapCapture.Capture((Bitmap)this._csharpBackgroundImage, _xBackImageSplitCount, _yBackImageSplitCount);
                this.Invalidate();
            }
        }

        #endregion 构造

        #region 用来标示是否鼠标正在悬浮在按钮上  true:悬浮在按钮上 false:鼠标离开了按钮

        //用来标示是否鼠标正在悬浮在按钮上  true:悬浮在按钮上 false:鼠标离开了按钮
        private bool isButtonMouseHover;

        #endregion 用来标示是否鼠标正在悬浮在按钮上  true:悬浮在按钮上 false:鼠标离开了按钮

        #region 用来标示是否鼠标点击了按钮  true：按下了按钮 false：松开了按钮

        //用来标示是否鼠标点击了按钮  true：按下了按钮 false：松开了按钮
        private bool isButtonMouseDown;

        #endregion 用来标示是否鼠标点击了按钮  true：按下了按钮 false：松开了按钮

        #region 重写鼠标悬浮的事件

        /// <summary>重写鼠标悬浮的事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            if (!_isSelected)
            {
                //当鼠标进入控件时，标示变量为进入了控件
                isButtonMouseHover = true;
                //刷新面板触发OnPaint重绘
                this.Invalidate();
                base.OnMouseEnter(e);
            }
        }

        #endregion 重写鼠标悬浮的事件

        #region 重写鼠标离开事件

        /// <summary>重写鼠标离开事件</summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            //当鼠标离开控件时，标示变量为离开了控件
            if (!_isSelected)
            {
                isButtonMouseHover = false;
                //刷新面板触发OnPaint重绘
                this.Invalidate();
                base.OnMouseLeave(e);
            }
        }

        #endregion 重写鼠标离开事件

        #region 按钮是否被选中

        /// <summary>按钮是否被选中</summary>
        private bool _isSelected = false;

        [Browsable(true)]
        [Description("是否被选中")]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    isButtonMouseDown = true;
                    //刷新面板触发OnPaint重绘
                    this.Invalidate();
                }
                else
                {
                    isButtonMouseDown = false;
                    //刷新面板触发OnPaint重绘
                    this.OnMouseLeave(null);
                    //this.Invalidate()();
                }
            }
        }

        #endregion 按钮是否被选中

        #region 重写鼠标按下的事件

        /// <summary>重写鼠标按下的事件</summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //当鼠标按下控件时，标示变量为按下了控件
            if (!_isSelected)
            {
                isButtonMouseDown = true;
                //刷新面板触发OnPaint重绘
                this.Invalidate();
                base.OnMouseDown(mevent);
            }
        }

        #endregion 重写鼠标按下的事件

        #region 重写鼠标松开的事件

        /// <summary>重写鼠标松开的事件</summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            //当鼠标松开时，标示变量为按下并松开了控件
            if (!_isSelected)
            {
                isButtonMouseDown = false;
                //刷新面板触发OnPaint重绘
                this.Invalidate();
                base.OnMouseUp(mevent);
            }
        }

        #endregion 重写鼠标松开的事件

        #region 重写绘制按钮事件

        /// <summary>重写绘制按钮事件</summary>
        /// <param name="pevent"></param>
        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (this._csharpBackgroundImage != null)
            {
                base.OnPaint(pevent);
                //因为上面调用了base会绘制原生控件 重刷一下背景清掉原生绘制 不然自己绘制的是重叠在原生绘制上
                base.OnPaintBackground(pevent);
                //得到绘画句柄
                Graphics g = pevent.Graphics;
                BufferedGraphicsContext bgc = new BufferedGraphicsContext();
                BufferedGraphics bg = bgc.Allocate(g, this.ClientRectangle);
                //定义字体格式
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                //处理热键 当Alt点下时
                sf.HotkeyPrefix = this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;
                //判断使用什么资源图
                Bitmap bmpDraw = null;// Properties.Resources.mb_btn_default;

                bmpDraw = BackImageSplitArray[0, 0];
                //如果禁用了，则使用禁用时的样式图片绘制，否则调用其他满足条件的样式图片绘制
                ////如果只有一行 那么类似这样---- 一行四列
                // if (!this.Enabled) bmpDraw = BackImageSplitArray[0, 0];

                #region 获取分割后图片有多少行，每行都多少个图片

                //获取分割后图片有多少行，每行都多少个图片
                int lineCount = BackImageSplitArray.GetLength(0);//多少行
                int splitlineCount = BackImageSplitArray.GetLength(1);//每行多少个
                //如果分为一行 则一行有多个
                if (lineCount == 1)
                {
                    //如果一行有两列 图片组成
                    if (splitlineCount == 2)
                    {
                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[0, 1];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[0, 1];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[0, 1];
                        }
                    }
                    else if (splitlineCount == 3)
                    {
                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[0, 1];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[0, 2];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[0, 2];
                        }
                    }
                    else
                    {
                        if (BackImageSplitArray.Length == 1) return;
                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[0, 1];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[0, 2];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[0, 3];
                        }
                    }
                }
                else
                {
                    //如果大于一行 那么类似这样
                    //-
                    //-
                    //-
                    //-
                    //四行一列
                    //如果有多行，则每行只有一个
                    if (lineCount == 2)
                    {
                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[1, 0];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[1, 0];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[1, 0];
                        }
                    }
                    else if (lineCount == 3)
                    {
                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[1, 0];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[2, 0];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[2, 0];
                        }
                    }
                    else
                    {
                        if (BackImageSplitArray.Length == 1) return;

                        if (isButtonMouseDown)
                        {
                            bmpDraw = BackImageSplitArray[1, 0];
                        }
                        else if (isButtonMouseHover)
                        {
                            bmpDraw = BackImageSplitArray[2, 0];
                        }
                        else if (this.Focused)
                        {
                            bmpDraw = BackImageSplitArray[3, 0];
                        }
                    }
                }

                #endregion 获取分割后图片有多少行，每行都多少个图片

                //绘制背景
                if (bmpDraw != null)
                {
                    g.DrawImage(bmpDraw, this.ClientRectangle);
                }
                //如果禁用
                if (!this.Enabled)
                {
                    //则绘制双重阴影文字
                    g.DrawString(this.Text, this.Font, Brushes.White, this.ClientRectangle, sf);
                    g.TranslateTransform(-1, -1);//左上移动一个单位坐标系
                    g.DrawString(this.Text, this.Font, Brushes.DarkGray, this.ClientRectangle, sf);
                    g.ResetTransform();
                    return;
                }
                //否则，默认绘制正常字体
                using (SolidBrush sb = new SolidBrush(this.ForeColor))
                {
                    g.DrawString(this.Text, this.Font, sb, this.ClientRectangle, sf);
                }
            }
            else
            {
                base.OnPaint(pevent);
            }
        }

        #endregion 重写绘制按钮事件
    }

    #endregion 按钮
}