using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.WinUI.Controls.TrackBar
{
    public class PaintTrackEventArgs : IDisposable
    {
        private Graphics _graphics;
        private Rectangle _trackRect;
        private IList<float> _tickPosList;

        #region 构造
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="g"></param>
        /// <param name="trackRect"></param>
        /// <param name="tickPosList"></param>
        public PaintTrackEventArgs(Graphics g, Rectangle trackRect, IList<float> tickPosList)
        {
            _graphics = g;
            _trackRect = trackRect;
            _tickPosList = tickPosList;
        }
        #endregion

        #region 返回画板
        /// <summary>
        /// 返回画板
        /// </summary>
        public Graphics Graphics
        {
            get { return _graphics; }
        }
        #endregion

        #region 返回范围
        /// <summary>
        /// 返回范围
        /// </summary>
        public Rectangle TrackRect
        {
            get { return _trackRect; }
        }
        #endregion

        #region 返回位置集合
        /// <summary>
        /// 返回位置集合
        /// </summary>
        public IList<float> TickPosList
        {
            get { return _tickPosList; }
        }
        #endregion

        #region 释放资源
        public virtual void Dispose()
        {
            _graphics = null;
            _tickPosList = null;
        }
        #endregion
    }
}
