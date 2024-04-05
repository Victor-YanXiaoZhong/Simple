using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace Simple.WinUI
{
    internal static class Win32Support
    {
        /// <summary>Enumeration to be used for those Win32 function that return BOOL</summary>
        public enum Bool
        {
            False = 0,
            True
        };

        /// <summary>
        /// Enumeration for the raster operations used in BitBlt. In C++ these are actually #define.
        /// But to use these constants with C#, a new enumeration type is defined.
        /// </summary>
        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020, /* dest = source                   */
            SRCPAINT = 0x00EE0086, /* dest = source OR dest           */
            SRCAND = 0x008800C6, /* dest = source AND dest          */
            SRCINVERT = 0x00660046, /* dest = source XOR dest          */
            SRCERASE = 0x00440328, /* dest = source AND (NOT dest )   */
            NOTSRCCOPY = 0x00330008, /* dest = (NOT source)             */
            NOTSRCERASE = 0x001100A6, /* dest = (NOT src) AND (NOT dest) */
            MERGECOPY = 0x00C000CA, /* dest = (source AND pattern)     */
            MERGEPAINT = 0x00BB0226, /* dest = (NOT source) OR dest     */
            PATCOPY = 0x00F00021, /* dest = pattern                  */
            PATPAINT = 0x00FB0A09, /* dest = DPSnoo                   */
            PATINVERT = 0x005A0049, /* dest = pattern XOR dest         */
            DSTINVERT = 0x00550009, /* dest = (NOT dest)               */
            BLACKNESS = 0x00000042, /* dest = BLACK                    */
            WHITENESS = 0x00FF0062, /* dest = WHITE                    */
        };

        public enum RedrawWindowFlags
        {
            RDW_INVALIDATE = 0x1,
            RDW_FRAME = 0x400
        }

        /// <summary>DeleteDC</summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        /// <summary>BitBlt</summary>
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjSource, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDCEx(IntPtr ptr);
    }

    public static class ControlAnimator
    {
        #region Enviroment fields

        private static readonly List<Thread> Threadpool = new List<Thread>();

        private static Form _overlay;

        #endregion Enviroment fields

        /// <summary>Defined the look of an AnimationThread.</summary>
        public enum DrawMode
        {
            /// <summary>Circle of dots</summary>
            Dots,

            /// <summary>Circle of lines</summary>
            Lines,

            /// <summary>Closed circle</summary>
            Circle,
        }

        /// <summary>Thread Methode with AnimationLogic</summary>
        /// <param name="paramArray"></param>
        private static void Animating(object paramArray)
        {
            #region Read Parameter

            var param = (object[])paramArray;
            //ControleSize
            var controlSize = (Size)param[0];
            //Drawmode (Lines, Dots, Circle)
            var drawmode = (DrawMode)param[2];
            //Penwidth to Draw
            var penWidth = (int)param[3];
            //Outer Circle Radius
            var outerCircle = (int)param[4];
            //Inner Circle Radius
            var innerCircle = (int)param[5];
            //DrawBufferSize
            var bufferSize = (outerCircle + penWidth) * 2;
            //Clockwise Rotation
            var clockwise = (bool)param[7];

            #endregion Read Parameter

            #region Set Enviromente

            //Get DeviceContex from Handle
            var ding = Win32Support.GetDCEx((IntPtr)param[1]);

            //Get Graphics from DeviceContext
            var g = Graphics.FromHdc(ding);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Make BackBuffer
            Image buffer = new Bitmap(bufferSize, bufferSize);
            var gBuffer = Graphics.FromImage(buffer);
            gBuffer.SmoothingMode = SmoothingMode.AntiAlias;

            #endregion Set Enviromente

            #region Set ColorArray

            var colors = new Color[12];
            for (var i = 0; i < 12; i++)
            {
                colors[i] = Color.FromArgb((255 - (i * 20)), (Color)param[6]);
            }

            #endregion Set ColorArray

            #region Translate ZeroPoint to Middle of Control

            gBuffer.TranslateTransform(dx: bufferSize / 2, dy: bufferSize / 2);
            g.TranslateTransform(dx: controlSize.Width / 2, dy: controlSize.Height / 2);

            #endregion Translate ZeroPoint to Middle of Control

            #region GetControlBackground

            Image background = new Bitmap(bufferSize, bufferSize);
            var gTmp = Graphics.FromImage(background);
            Win32Support.BitBlt(gTmp.GetHdc(), 0, 0, bufferSize, bufferSize, g.GetHdc(), (controlSize.Width / 2) - (bufferSize / 2), (controlSize.Height / 2) - (bufferSize / 2), Win32Support.TernaryRasterOperations.SRCCOPY);
            g.ReleaseHdc();
            gTmp.ReleaseHdc();

            #endregion GetControlBackground

            #region AnimationLoop

            while (true)
            {
                //Catch Handle Exception
                try
                {
                    #region Draw Background

                    gBuffer.DrawImage(background, -(bufferSize / 2), -(bufferSize / 2));

                    #endregion Draw Background

                    #region Draw Animation

                    for (int i = 0; i < 12; i++)
                    {
                        switch (drawmode)
                        {
                            case DrawMode.Dots:
                                gBuffer.FillEllipse(new SolidBrush(colors[i]), 0, outerCircle - penWidth, penWidth * 2, penWidth * 2);
                                break;

                            case DrawMode.Lines:
                                var pen = new Pen(new SolidBrush(colors[i]), penWidth)
                                {
                                    StartCap = LineCap.Round,
                                    EndCap = LineCap.Round
                                };
                                gBuffer.DrawLine(pen, new Point(0, innerCircle), new Point(0, outerCircle));
                                break;

                            case DrawMode.Circle:
                                gBuffer.DrawArc(new Pen(colors[i], penWidth * 2), new Rectangle(-(outerCircle), -(outerCircle), outerCircle * 2, outerCircle * 2), 0, 31);
                                break;
                        }
                        if (clockwise)
                        {
                            gBuffer.RotateTransform(-30);
                        }
                        else
                        {
                            gBuffer.RotateTransform(30);
                        }
                    }

                    #endregion Draw Animation

                    #region Draw BackBuffer

                    //Draw Buffer
                    g.DrawImage(buffer, new Point(-(bufferSize / 2), -(bufferSize / 2)));

                    #endregion Draw BackBuffer

                    #region Colorrotation

                    //Array Rotation
                    var tmp = new Color[12];
                    colors.CopyTo(tmp, 0);
                    colors[0] = tmp[1];
                    colors[1] = tmp[2];
                    colors[2] = tmp[3];
                    colors[3] = tmp[4];
                    colors[4] = tmp[5];
                    colors[5] = tmp[6];
                    colors[6] = tmp[7];
                    colors[7] = tmp[8];
                    colors[8] = tmp[9];
                    colors[9] = tmp[10];
                    colors[10] = tmp[11];
                    colors[11] = tmp[0];

                    #endregion Colorrotation

                    //Interval
                    Thread.Sleep(100);
                }
                catch (Exception)
                {
                    #region Release from Threadpool

                    //Release from Threadpool
                    _findThreadParameter = ((IntPtr)param[1]).ToString();
                    Threadpool.RemoveAt(Threadpool.FindIndex(FindThread));

                    #endregion Release from Threadpool
                }

                #region Test Thread in Threadpool

                //If not in Threadpool end Thread
                _findThreadParameter = ((IntPtr)param[1]).ToString();
                if (!Threadpool.Exists(FindThread))
                {
                    break;
                }

                #endregion Test Thread in Threadpool
            }

            #endregion AnimationLoop

            #region Resourcen freigeben

            //Resourcen freigeben
            background.Dispose();
            buffer.Dispose();
            gBuffer.Dispose();
            g.Dispose();
            Win32Support.DeleteDC(ding);
            ding = IntPtr.Zero;

            #endregion Resourcen freigeben
        }

        /// <summary>Calculate Size of Buffer</summary>
        /// <param name="size">ControlSize</param>
        /// <returns>Outer Circle Radius</returns>
        private static int GetOuterCircle(Size size)
        {
            int result;

            if (size.Height > 50 && size.Width > 50)
            {
                result = 25;
            }
            else
            {
                if (size.Height < size.Width)
                {
                    result = (size.Height - 10) / 2;
                }
                else
                {
                    result = (size.Width - 10) / 2;
                }
            }

            //Results below 1 will cause "Parameter is not valid" errors
            if (result <= 0)
                result = 1;

            return result;
        }

        /// <summary>Start an Animation Thread for the given Control.</summary>
        /// <remarks>[Internal used]</remarks>
        /// <param name="size">Size of Bounded Rectangle</param>
        /// <param name="handle">Handle to DrawControl</param>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="pen">PenWidth</param>
        /// <param name="outer">Outer Circle Radius</param>
        /// <param name="inner">Inner Circle Radius</param>
        /// <param name="name">ThreadName</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Clockwise Rotation</param>
        private static void ThreadStart(Size size, IntPtr handle, DrawMode drawMode, int pen, int outer, int inner, string name, Color color, bool clockwise)
        {
            var thread = new Thread(new ParameterizedThreadStart(Animating)) { Name = name };

            Threadpool.Add(thread);

            thread.Start(new object[8] { size, handle, drawMode, pen, outer, inner, color, clockwise });
        }

        public static void StartFormAnimating(this Form form, Action action)
        {
            form.Enabled = false;
            form.Opacity = 0.95;

            var mask = new Panel();
            mask.Dock = DockStyle.Fill;
            form.Controls.Add(mask);

            StartAnimating(form.Handle, form.Size, DrawMode.Circle,
                Color.Red, true);

            //Task.Run(() => { Task.Delay(10 * 1000); });
        }

        /// <summary>Start an Animation Thread for the given Control.</summary>
        /// <param name="control">DrawControl</param>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="penWidth">PenWidth</param>
        /// <param name="outerCircle">Outer Circle Radius</param>
        /// <param name="innerCircle">Inner Circle Radius</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Clockwise Rotation</param>
        public static void StartAnimating(Control control, DrawMode drawMode, int penWidth, int outerCircle, int innerCircle, Color color, bool clockwise)
        {
            if (control.IsHandleCreated)
            {
                _findThreadParameter = control.Handle.ToString();
                if (!Threadpool.Exists(FindThread))
                {
                    control.Enabled = false;
                    control.Refresh();

                    ThreadStart(control.Size, control.Handle, drawMode, penWidth, outerCircle, innerCircle, control.Handle.ToString(), color, clockwise);
                }
            }
            else
            {
                throw new NullReferenceException("No Handle avalible.");
            }
        }

        /// <summary>Locked the Screen and start an Animation Thread on the Screen.</summary>
        /// <remarks>Use [ESC] for Cancel</remarks>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Roatate clockwise or anit-clockwise?</param>
        public static void FullScreenLock(DrawMode drawMode, Color color, bool clockwise)
        {
            if (_overlay == null)
            {
                _overlay = new Form
                {
                    TopMost = true,
                    ShowInTaskbar = false,
                    FormBorderStyle = FormBorderStyle.None,
                    WindowState = FormWindowState.Maximized,
                    BackColor = Color.Black,
                    Opacity = 0.5
                };

                _overlay.Show();

                int width = Screen.PrimaryScreen.Bounds.Width;
                int height = Screen.PrimaryScreen.Bounds.Height;
                Size clipBounds = new Size(width, height);

                _findThreadParameter = IntPtr.Zero.ToString();
                if (!Threadpool.Exists(FindThread))
                {
                    int outer = GetOuterCircle(clipBounds);
                    ThreadStart(clipBounds, IntPtr.Zero, drawMode, outer / 5, outer, outer / 2, IntPtr.Zero.ToString(), color, clockwise);
                }
            }
        }

        /// <summary>Release the Screen and stop Animation Thread on the Screen.</summary>
        public static void ReleaseFullScreenLock()
        {
            if (_overlay != null)
            {
                _findThreadParameter = IntPtr.Zero.ToString();
                if (Threadpool.Exists(FindThread))
                {
                    Threadpool.RemoveAt(Threadpool.FindIndex(FindThread));
                }

                _overlay.Close();
                _overlay = null;
            }
        }

        /// <summary>Start an Animation Thread for the given Control.</summary>
        /// <param name="control">DrawControl</param>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Clockwise Rotation</param>
        public static void StartAnimating(Control control, DrawMode drawMode, Color color, bool clockwise)
        {
            if (control.IsHandleCreated)
            {
                _findThreadParameter = control.Handle.ToString();
                if (!Threadpool.Exists(FindThread))
                {
                    int outer = GetOuterCircle(control.Size);

                    control.Enabled = false;
                    control.Refresh();

                    ThreadStart(control.Size, control.Handle, drawMode, outer / 5, outer, outer / 2, control.Handle.ToString(), color, clockwise);
                }
            }
            else
            {
                throw new NullReferenceException("No Handle avalible.");
            }
        }

        /// <summary>Start an Animation Thread for the given Control.</summary>
        /// <param name="handle">Handle to DrawControl</param>
        /// <param name="clipBounds">Size of Bounded Rectangle</param>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="penWidth">PenWidth</param>
        /// <param name="outerCircle">Outer Circle Radius</param>
        /// <param name="innerCircle">Inner Circle Radius</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Clockwise Rotation</param>
        public static void StartAnimating(IntPtr handle, Size clipBounds, DrawMode drawMode, int penWidth, int outerCircle, int innerCircle, Color color, bool clockwise)
        {
            _findThreadParameter = handle.ToString();
            if (!Threadpool.Exists(FindThread))
            {
                ThreadStart(clipBounds, handle, drawMode, penWidth, outerCircle, innerCircle, handle.ToString(), color, clockwise);
            }
        }

        /// <summary>Start an Animation Thread for the given Control.</summary>
        /// <param name="handle">Handle to DrawControl</param>
        /// <param name="clipBounds">Size of Bounded Rectangle</param>
        /// <param name="drawMode">Drawmode</param>
        /// <param name="color">BaseColor</param>
        /// <param name="clockwise">Clockwise Rotation</param>
        public static void StartAnimating(IntPtr handle, Size clipBounds, DrawMode drawMode, Color color, bool clockwise)
        {
            _findThreadParameter = handle.ToString();
            if (!Threadpool.Exists(FindThread))
            {
                int outer = GetOuterCircle(clipBounds);
                ThreadStart(clipBounds, handle, drawMode, outer / 5, outer, outer / 2, handle.ToString(), color, clockwise);
            }
        }

        /// <summary>Stops a running Animation Thread for the given Control.</summary>
        /// <param name="control">DrawControl</param>
        public static void StopAnimating(Control control)
        {
            if (control.IsHandleCreated)
            {
                _findThreadParameter = control.Handle.ToString();
                if (Threadpool.Exists(FindThread))
                {
                    Threadpool.RemoveAt(Threadpool.FindIndex(FindThread));

                    control.Enabled = true;
                    control.Invalidate();
                }
            }
        }

        /// <summary>Stops a running Animation Thread for the given Handle.</summary>
        /// <remarks>After stopping by Handle, you have to repaint the object by yourself!</remarks>
        /// <param name="handle">Handle to DrawControl</param>
        public static void StopAnimating(IntPtr handle)
        {
            _findThreadParameter = handle.ToString();
            if (Threadpool.Exists(FindThread))
            {
                Threadpool.RemoveAt(Threadpool.FindIndex(FindThread));
            }
        }

        #region Helper method

        private static string _findThreadParameter = String.Empty;

        private static bool FindThread(Thread thread)
        {
            if (thread.Name == _findThreadParameter)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Helper method
    }
}