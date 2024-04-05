namespace Simple.WinUI
{
    #region 调用系统API生成的图像对象

    /// <summary>WWW.CSharpSkin.COM 调用系统API生成的图像对象</summary>
    public class ImageDc
    {
        private int _height = 0;
        private int _width = 0;
        private IntPtr _intPtr = IntPtr.Zero;
        private IntPtr _bmpIntPtr = IntPtr.Zero;

        private IntPtr _bmpOriginalIntPtr = IntPtr.Zero;

        public IntPtr IntPtr
        {
            get { return _intPtr; }
        }

        public IntPtr BmpIntPtr
        {
            get { return _bmpIntPtr; }
        }

        #region 构造

        /// <summary>构造</summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hBmp"></param>
        public ImageDc(int width, int height, IntPtr hBmp)
        {
            CreateImageDc(width, height, hBmp);
        }

        public ImageDc(int width, int height)
        {
            CreateImageDc(width, height, IntPtr.Zero);
        }

        #endregion 构造

        #region 创建图像句柄对象

        /// <summary>创建图像句柄对象</summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="hBmp"></param>
        private void CreateImageDc(int width, int height, IntPtr hBmp)
        {
            IntPtr intPtr = IntPtr.Zero;
            intPtr = WinApi.CreateDCA("DISPLAY", "", "", 0);
            _intPtr = WinApi.CreateCompatibleDC(intPtr);
            if (hBmp != IntPtr.Zero)
            {
                _bmpIntPtr = hBmp;
            }
            else
            {
                _bmpIntPtr = WinApi.CreateCompatibleBitmap(intPtr, width, height);
            }
            _bmpOriginalIntPtr = WinApi.SelectObject(_intPtr, _bmpIntPtr);
            if (_bmpOriginalIntPtr == IntPtr.Zero)
            {
                this.Destroy();
            }
            else
            {
                _width = width;
                _height = height;
            }
            WinApi.DeleteDC(intPtr);
            intPtr = IntPtr.Zero;
        }

        #endregion 创建图像句柄对象

        #region 根据句柄销毁对象

        /// <summary>根据句柄销毁对象</summary>
        private void Destroy()
        {
            if (_bmpOriginalIntPtr != IntPtr.Zero)
            {
                WinApi.SelectObject(_intPtr, _bmpOriginalIntPtr);
                _bmpOriginalIntPtr = IntPtr.Zero;
            }
            if (_bmpIntPtr != IntPtr.Zero)
            {
                WinApi.DeleteObject(_bmpIntPtr);
                _bmpIntPtr = IntPtr.Zero;
            }
            if (_intPtr != IntPtr.Zero)
            {
                WinApi.DeleteDC(_intPtr);
                _intPtr = IntPtr.Zero;
            }
        }

        #endregion 根据句柄销毁对象

        #region 销毁对象

        /// <summary>销毁对象</summary>
        public void Dispose()
        {
            this.Destroy();
        }

        #endregion 销毁对象
    }

    #endregion 调用系统API生成的图像对象
}