using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using Simple.WinUI.Controls.TrackBar.ProperityGrid;
using System.Drawing.Design;

namespace Simple.WinUI.Controls.TrackBar
{
    #region 带有样式的滑动条

    /// <summary>WWW.CSharpSkin.COM 带有样式的滑动条</summary>
    public class UTrackBar : System.Windows.Forms.TrackBar
    {
        #region 构造

        /// <summary>构造</summary>
        public UTrackBar() : base()
        { }

        #endregion 构造

        #region 颜色样式

        /// <summary>颜色样式</summary>

        public TrackBarColorStyle _trackBarColorStyle = new TrackBarColorStyle();

        [Browsable(true)]
        [Localizable(true)]
        [Description("颜色样式")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor(typeof(TrackBarColorStylePropertyEditor), typeof(UITypeEditor))]
        public TrackBarColorStyle TrackBarColorStyle
        {
            get
            {
                if (_trackBarColorStyle == null)
                {
                    _trackBarColorStyle = new TrackBarColorStyle();
                }
                return _trackBarColorStyle;
            }
            set
            {
                _trackBarColorStyle = value;
                base.Invalidate();
            }
        }

        #endregion 颜色样式

        #region 绘制背景

        /// <summary>绘制背景</summary>
        /// <param name="pevent"></param>
        protected void OnPaintEventBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
        }

        #endregion 绘制背景

        #region 是否重绘

        /// <summary>是否重绘</summary>
        protected bool _isOpenPainting = true;

        #endregion 是否重绘

        #region 监听系统消息，根据消息标识重绘滑动条

        /// <summary>监听系统消息，根据消息标识重绘滑动条</summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WinApi.WM_PAINT:
                    if (_isOpenPainting)
                    {
                        _isOpenPainting = false;
                        PAINTSTRUCT ps = new PAINTSTRUCT();
                        WinApi.BeginPaint(m.HWnd, ref ps);
                        DrawTrackBar(m.HWnd);
                        WinApi.ValidateRect(m.HWnd, ref ps.rcPaint);
                        WinApi.EndPaint(m.HWnd, ref ps);
                        _isOpenPainting = true;
                        m.Result = WMRESULT.TRUE;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 监听系统消息，根据消息标识重绘滑动条

        #region 判断当前鼠标是否在滑动条范围内

        /// <summary>判断当前鼠标是否在滑动条范围内</summary>
        /// <param name="thumbRect"></param>
        /// <returns></returns>
        private bool IsMouseHovering(RECT tempRect)
        {
            RECT windowRect = new RECT();
            Point point = new Point();
            WinApi.GetWindowRect(base.Handle, ref windowRect);
            WinApi.OffsetRect(
                ref tempRect, windowRect.Left, windowRect.Top);
            WinApi.GetCursorPos(ref point);
            if (WinApi.PtInRect(ref tempRect, point))
            {
                return true;
            }
            return false;
        }

        #endregion 判断当前鼠标是否在滑动条范围内

        #region 绘制滑动条

        /// <summary>绘制滑动条</summary>
        /// <param name="intPtr"></param>
        protected void DrawTrackBar(IntPtr intPtr)
        {
            TrackBarState state = TrackBarState.Normal;
            bool horizontal = base.Orientation == Orientation.Horizontal;
            ImageDc imageDc = new ImageDc(base.Width, base.Height);
            RECT trackRect = new RECT();
            RECT tempRect = new RECT();
            Graphics g = Graphics.FromHdc(imageDc.IntPtr);
            WinApi.SendMessage(intPtr, WinApi.TBM_GETCHANNELRECT, 0, ref trackRect);
            WinApi.SendMessage(intPtr, WinApi.TBM_GETTHUMBRECT, 0, ref tempRect);
            Rectangle trackRectangle = horizontal ?
                trackRect.Rect :
                Rectangle.FromLTRB(
                trackRect.Top, trackRect.Left,
                trackRect.Bottom, trackRect.Right);

            if (IsMouseHovering(tempRect))
            {
                if (MouseState.IsMouseLeftKeyPress())
                {
                    state = TrackBarState.Pressed;
                }
                else
                {
                    state = TrackBarState.Hover;
                }
            }
            using (PaintEventArgs pe = new PaintEventArgs(
                g, ClientRectangle))
            {
                OnPaintEventBackground(pe);
            }
            int ticks = WinApi.SendMessage(intPtr, WinApi.TBM_GETNUMTICS, 0, 0);
            if (ticks > 0)
            {
                List<float> tickPosList = new List<float>(ticks);
                int tempOffset = horizontal ?
                    tempRect.Rect.Width : tempRect.Rect.Height;
                int trackWidth = trackRect.Right - trackRect.Left;
                float tickSpace = (trackWidth - tempOffset) / (float)(ticks - 1);
                float offset = trackRect.Left + tempOffset / 2f;
                for (int pos = 0; pos < ticks; pos++)
                {
                    tickPosList.Add(offset + tickSpace * pos);
                }
                using (PaintTrackEventArgs pte = new PaintTrackEventArgs(g, trackRectangle, tickPosList))
                {
                    OnPaintTrack(pte);
                }
            }

            using (PaintEventArgs pe = new PaintEventArgs(
                g, trackRectangle))
            {
                OnPaintTick(pe);
            }

            using (PaintStateEventArgs pe = new PaintStateEventArgs(g, tempRect.Rect, state))
            {
                OnPaintState(pe);
            }

            g.Dispose();
            IntPtr hDC = WinApi.GetDC(intPtr);
            WinApi.BitBlt(
                    hDC, 0, 0, base.Width, base.Height,
                    imageDc.IntPtr, 0, 0, 0xCC0020);
            WinApi.ReleaseDC(intPtr, hDC);
            imageDc.Dispose();
        }

        #endregion 绘制滑动条

        #region 绘制滑标

        /// <summary>绘制滑标</summary>
        /// <param name="e"></param>
        protected virtual void OnPaintTrack(PaintTrackEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle trackRect = e.TrackRect;
            bool bHorizontal = base.Orientation == Orientation.Horizontal;
            int posFirst = 0;
            int posSecond = 0;
            bool bTickBoth = base.TickStyle == TickStyle.Both;
            if (bHorizontal)
            {
                switch (base.TickStyle)
                {
                    case TickStyle.TopLeft:
                        posFirst = trackRect.Top - 15;
                        break;

                    case TickStyle.BottomRight:
                        posFirst = trackRect.Bottom + 13;
                        break;

                    case TickStyle.Both:
                        posFirst = trackRect.Top - 15;
                        posSecond = trackRect.Bottom + 13;
                        break;
                }
            }
            else
            {
                switch (base.TickStyle)
                {
                    case TickStyle.TopLeft:
                        posFirst = trackRect.Left - 15;
                        break;

                    case TickStyle.BottomRight:
                        posFirst = trackRect.Right + 13;
                        break;

                    case TickStyle.Both:
                        posFirst = trackRect.Left - 15;
                        posSecond = trackRect.Right + 13;
                        break;
                }
            }

            Pen lightPen = new Pen(this._trackBarColorStyle.TrackLightColor);
            Pen darkPen = new Pen(this._trackBarColorStyle.TrackDarkColor);
            if (bHorizontal)
            {
                foreach (int tickPos in e.TickPosList)
                {
                    g.DrawLine(
                        lightPen, new Point(tickPos, posFirst),
                        new Point(tickPos, posFirst + 2));
                    g.DrawLine(darkPen, new Point(tickPos + 1, posFirst), new Point(tickPos + 1, posFirst + 2));
                    if (bTickBoth)
                    {
                        g.DrawLine(lightPen, new Point(tickPos, posSecond), new Point(tickPos, posSecond + 2));
                        g.DrawLine(darkPen, new Point(tickPos + 1, posSecond), new Point(tickPos + 1, posSecond + 2));
                    }
                }
            }
            else
            {
                foreach (int tickPos in e.TickPosList)
                {
                    g.DrawLine(lightPen, new Point(posFirst + 2, tickPos), new Point(posFirst, tickPos));
                    g.DrawLine(darkPen, new Point(posFirst, tickPos + 1), new Point(posFirst + 2, tickPos + 1));
                    if (bTickBoth)
                    {
                        g.DrawLine(lightPen, new Point(posSecond + 2, tickPos), new Point(posSecond, tickPos));
                        g.DrawLine(darkPen, new Point(posSecond, tickPos + 1), new Point(posSecond + 2, tickPos + 1));
                    }
                }
            }
            lightPen.Dispose();
            darkPen.Dispose();
        }

        #endregion 绘制滑标

        #region 绘制滑动条

        /// <summary>绘制滑动条</summary>
        /// <param name="e"></param>
        protected virtual void OnPaintTick(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            bool horizontal = base.Orientation == Orientation.Horizontal;
            float mode = horizontal ? 0f : 270f;

            if (horizontal)
            {
                rect.Inflate(0, 1);
            }
            else
            {
                rect.Inflate(1, 0);
            }

            //SmoothingModeGraphics sg = new SmoothingModeGraphics(g);

            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, 4, RoundStyle.All, true))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, this._trackBarColorStyle.TrackBeginColor, this._trackBarColorStyle.TrackEndColor, mode))
                {
                    g.FillPath(brush, path);
                }
                using (Pen pen = new Pen(this._trackBarColorStyle.TrackBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathManager.CreatePath(rect, 4, RoundStyle.All, true))
            {
                using (Pen pen = new Pen(this._trackBarColorStyle.TrackInnerBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
            //sg.Dispose();
        }

        #endregion 绘制滑动条

        #region 绘制滑动条鼠标状态

        /// <summary>绘制滑动条鼠标状态</summary>
        /// <param name="e"></param>
        protected virtual void OnPaintState(PaintStateEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;
            TrackBarState state = e.TrackBarState;
            TrackTickArrowDirection direction = TrackTickArrowDirection.None;
            Color begin = _trackBarColorStyle.TrackBackgroundNormalColor;
            Color end = _trackBarColorStyle.TrackInnerBorderColor;
            Color border = _trackBarColorStyle.TrackBorderNormalColor;
            float mode = base.Orientation == Orientation.Horizontal ? 90f : 0f;
            switch (base.Orientation)
            {
                case Orientation.Horizontal:
                    switch (base.TickStyle)
                    {
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = TrackTickArrowDirection.Down;
                            break;

                        case TickStyle.TopLeft:
                            direction = TrackTickArrowDirection.Up;
                            break;

                        case TickStyle.Both:
                            direction = TrackTickArrowDirection.None;
                            break;
                    }
                    break;

                case Orientation.Vertical:
                    switch (base.TickStyle)
                    {
                        case TickStyle.TopLeft:
                            direction = TrackTickArrowDirection.Left;
                            break;

                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = TrackTickArrowDirection.Right;
                            break;

                        case TickStyle.Both:
                            direction = TrackTickArrowDirection.None;
                            break;
                    }
                    break;
            }

            switch (state)
            {
                case TrackBarState.Hover:
                    begin = _trackBarColorStyle.TrackBackgroundHoverColor;//.ThumbBackHover;
                    border = _trackBarColorStyle.TrackBorderHoverColor;//.ThumbBorderHover;
                    break;
            }

            g.SmoothingMode = SmoothingMode.HighQuality;
            using (GraphicsPath path = GraphicsPathManager.CreateTrackBarTickPath(rect, direction))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(rect, begin, end, mode))
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0, .2f, .5f, .8f, 1f };
                    blend.Factors = new float[] { 1f, .7f, 0, .7f, 1f };
                    brush.Blend = blend;
                    g.FillPath(brush, path);
                }
                using (Pen pen = new Pen(border))
                {
                    g.DrawPath(pen, path);
                }
            }
            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathManager.CreateTrackBarTickPath(rect, direction))
            {
                using (Pen pen = new Pen(_trackBarColorStyle.TrackInnerBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        #endregion 绘制滑动条鼠标状态
    }

    #endregion 带有样式的滑动条
}