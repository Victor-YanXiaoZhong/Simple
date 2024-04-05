using Microsoft.Win32.SafeHandles;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.WinUI.Helper
{
    internal class SafePrinter : SafeHandleZeroOrMinusOneIsInvalid
    {
        private SafePrinter(IntPtr hPrinter)
            : base(true)
        {
            handle = hPrinter;
        }

        private static IEnumerable<string> ReadMultiSz(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
            {
                yield break;
            }

            var builder = new StringBuilder();
            var pos = ptr;

            while (true)
            {
                var c = (char)Marshal.ReadInt16(pos);

                if (c == '\0')
                {
                    if (builder.Length == 0)
                    {
                        break;
                    }

                    yield return builder.ToString();
                    builder = new StringBuilder();
                }
                else
                {
                    builder.Append(c);
                }

                pos += 2;
            }
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
            {
                return false;
            }

            var result = WinPrintApi.ClosePrinter(handle) != 0;
            handle = IntPtr.Zero;

            return result;
        }

        public static SafePrinter OpenPrinter(string printerName, ref PRINTER_DEFAULTS defaults)
        {
            IntPtr hPrinter;

            if (WinPrintApi.OpenPrinterW(printerName, out hPrinter, ref defaults) == 0)
            {
                throw new Win32Exception();
            }

            return new SafePrinter(hPrinter);
        }

        public uint StartDocPrinter(DOC_INFO_1 di1)
        {
            var id = WinPrintApi.StartDocPrinterW(handle, 1, ref di1);
            if (id == 0)
            {
                if (Marshal.GetLastWin32Error() == 1804)
                {
                    throw new Exception("The specified datatype is invalid, try setting 'Enable advanced printing features' in printer properties.", new Win32Exception());
                }
                throw new Win32Exception();
            }

            return id;
        }

        public void EndDocPrinter()
        {
            if (WinPrintApi.EndDocPrinter(handle) == 0)
            {
                throw new Win32Exception();
            }
        }

        public void StartPagePrinter()
        {
            if (WinPrintApi.StartPagePrinter(handle) == 0)
            {
                throw new Win32Exception();
            }
        }

        public void EndPagePrinter()
        {
            if (WinPrintApi.EndPagePrinter(handle) == 0)
            {
                throw new Win32Exception();
            }
        }

        public void WritePrinter(byte[] buffer, int size)
        {
            int written = 0;
            if (WinPrintApi.WritePrinter(handle, buffer, size, ref written) == 0)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public IEnumerable<string> GetPrinterDriverDependentFiles()
        {
            int bufferSize = 0;

            if (WinPrintApi.GetPrinterDriver(handle, null, 3, IntPtr.Zero, 0, ref bufferSize) != 0 || Marshal.GetLastWin32Error() != 122) // 122 = ERROR_INSUFFICIENT_BUFFER
            {
                throw new Win32Exception();
            }

            var ptr = Marshal.AllocHGlobal(bufferSize);

            try
            {
                if (WinPrintApi.GetPrinterDriver(handle, null, 3, ptr, bufferSize, ref bufferSize) == 0)
                {
                    throw new Win32Exception();
                }

                var di3 = (DRIVER_INFO_3)Marshal.PtrToStructure(ptr, typeof(DRIVER_INFO_3));

                return ReadMultiSz(di3.pDependentFiles).ToList(); // We need a list because FreeHGlobal will be called on return
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }

    public class DocRawPrint
    {
        public static string DefaultPrint;

        public DocRawPrint(string printer = "")
        {
            if (string.IsNullOrEmpty(printer))
            {
                DefaultPrint = new PrinterSettings().PrinterName;
            }
            else { DefaultPrint = printer; }
        }

        public event Action<JobCreatedEventArgs> OnJobCreated;

        private static bool IsXPSDriver(SafePrinter printer)
        {
            var files = printer.GetPrinterDriverDependentFiles();

            return files.Any(f => f.EndsWith("pipelineconfig.xml", StringComparison.InvariantCultureIgnoreCase));
        }

        private static void PagePrinter(SafePrinter printer, Stream stream, int pagecount)
        {
            printer.StartPagePrinter();

            try
            {
                WritePrinter(printer, stream);
            }
            finally
            {
                printer.EndPagePrinter();
            }

            // Fix the page count in the final document
            for (int i = 1; i < pagecount; i++)
            {
                printer.StartPagePrinter();
                printer.EndPagePrinter();
            }
        }

        private static void WritePrinter(SafePrinter printer, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            const int bufferSize = 1048576;
            var buffer = new byte[bufferSize];

            int read;
            while ((read = stream.Read(buffer, 0, bufferSize)) != 0)
            {
                printer.WritePrinter(buffer, read);
            }
        }

        private void DocPrinter(SafePrinter printer, string documentName, string dataType, Stream stream, bool paused, int pagecount, string printerName)
        {
            var di1 = new DOC_INFO_1
            {
                pDataType = dataType,
                pDocName = documentName,
            };

            var id = printer.StartDocPrinter(di1);

            if (paused)
            {
                WinPrintApi.SetJob(printer.DangerousGetHandle(), id, 0, IntPtr.Zero, (int)JobControl.Pause);
            }

            OnJobCreated?.Invoke(new JobCreatedEventArgs { Id = id, PrinterName = printerName });

            try
            {
                PagePrinter(printer, stream, pagecount);
            }
            finally
            {
                printer.EndDocPrinter();
            }
        }

        public void PrintRawFile(string path, bool paused = false)
        {
            PrintRawFile(path, path, paused);
        }

        public void PrintRawFile(string path, string documentName, bool paused = false)
        {
            using (var stream = File.OpenRead(path))
            {
                PrintRawStream(stream, documentName, paused);
            }
        }

        public void PrintRawStream(Stream stream, string documentName, bool paused)
        {
            PrintRawStream(stream, documentName, paused, 1);
        }

        public void PrintRawStream(Stream stream, string documentName, bool paused, int pagecount)
        {
            var defaults = new PRINTER_DEFAULTS
            {
                DesiredPrinterAccess = PRINTER_ACCESS_MASK.PRINTER_ACCESS_USE
            };

            using (var safePrinter = SafePrinter.OpenPrinter(DefaultPrint, ref defaults))
            {
                DocPrinter(safePrinter, documentName, IsXPSDriver(safePrinter) ? "XPS_PASS" : "RAW", stream, paused, pagecount, DefaultPrint);
            }
        }
    }

    /// <summary>打印任务参数</summary>
    public class JobCreatedEventArgs
    {
        public uint Id { get; set; }
        public string PrinterName { get; set; }
    }

    #region win32 参数

    // ReSharper disable InconsistentNaming
    // ReSharper disable FieldCanBeMadeReadOnly.Local
    [Flags]
    internal enum PRINTER_ACCESS_MASK : uint
    {
        PRINTER_ACCESS_ADMINISTER = 0x00000004,
        PRINTER_ACCESS_USE = 0x00000008,
        PRINTER_ACCESS_MANAGE_LIMITED = 0x00000040,
        PRINTER_ALL_ACCESS = 0x000F000C,
    }

    public enum JobControl
    {
        Pause = 0x01,
        Resume = 0x02,
        Cancel = 0x03,
        Restart = 0x04,
        Delete = 0x05,
        Retain = 0x08,
        Release = 0x09,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct PRINTER_DEFAULTS
    {
        public string pDatatype;

        private IntPtr pDevMode;

        public PRINTER_ACCESS_MASK DesiredPrinterAccess;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DOC_INFO_1
    {
        public string pDocName;

        public string pOutputFile;

        public string pDataType;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct DRIVER_INFO_3
    {
        public uint cVersion;
        public string pName;
        public string pEnvironment;
        public string pDriverPath;
        public string pDataFile;
        public string pConfigFile;
        public string pHelpFile;
        public IntPtr pDependentFiles;
        public string pMonitorName;
        public string pDefaultDataType;
    }

    internal class WinPrintApi
    {
        [DllImport("winspool.drv", SetLastError = true)]
        public static extern int ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetPrinterDriver(IntPtr hPrinter, string pEnvironment, int Level, IntPtr pDriverInfo, int cbBuf, ref int pcbNeeded);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint StartDocPrinterW(IntPtr hPrinter, uint level, [MarshalAs(UnmanagedType.Struct)] ref DOC_INFO_1 di1);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int WritePrinter(IntPtr hPrinter, [In, Out] byte[] pBuf, int cbBuf, ref int pcWritten);

        [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int OpenPrinterW(string pPrinterName, out IntPtr phPrinter, ref PRINTER_DEFAULTS pDefault);

        [DllImport("winspool.drv", EntryPoint = "SetJobA", SetLastError = true)]
        public static extern int SetJob(IntPtr hPrinter, uint JobId, uint Level, IntPtr pJob, uint Command_Renamed);
    }

    #endregion win32 参数
}