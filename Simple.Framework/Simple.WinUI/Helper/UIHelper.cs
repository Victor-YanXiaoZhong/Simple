using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple.WinUI.Helper
{
    /// <summary>UI帮助类</summary>
    public static class UIHelper
    {
        #region MessageBox

        public static void AlertInfo(this Form owner, String text, String caption = "消息")
        {
            SafeInvoke(owner, () =>
            {
                MessageBoxHelper.ShowInfo(text, caption: caption);
            });
        }

        public static void AlertToast(this Form owner, String text, String caption = "消息", TimeSpan? delayTime = null)
        {
            SafeInvoke(owner, () =>
            {
                MessageBoxHelper.ShowTost(text, caption: caption, delayTime: delayTime);
            });
        }

        public static void AlertWarning(this Form owner, String text, String caption = "警告")
        {
            SafeInvoke(owner, () =>
            {
                MessageBoxHelper.ShowWarning(text, caption: caption);
            });
        }

        public static void AlertError(this Form owner, String text, String caption = "错误")
        {
            SafeInvoke(owner, () =>
            {
                MessageBoxHelper.ShowError(text, caption: caption);
            });
        }

        public static void AlertError(this Form owner, Exception ex, string text = "", String caption = "错误",
            Boolean throwError = false)
        {
            SafeInvoke(owner, () =>
            {
                MessageBoxHelper.ShowError(text, caption: caption, exception: ex);
            });
            if (throwError) { throw ex; }
        }

        public static void AlertError(this Exception ex, string text = "", String caption = "错误", Boolean throwError = false)
        {
            MessageBoxHelper.ShowError(text, caption: caption, exception: ex);
            if (throwError) { throw ex; }
        }

        public static void AlertOperationResult(this Form owner, Tuple<Boolean, String> result)
        {
            if (result.Item1) { owner.AlertInfo(result.Item2); } else { owner.AlertWarning(result.Item2); }
        }

        public static void AlertOperationResult(this Form owner, Boolean flag, String trueMsg, String falseMsg)
        {
            if (flag) { owner.AlertInfo(trueMsg); } else { owner.AlertWarning(falseMsg); }
        }

        public static Boolean Confirm(this Form owner, String text, String caption = "确认")
        {
            Boolean flag = false;
            SafeInvoke(owner, () =>
            {
                flag = DialogResult.Yes == MessageBoxHelper.ShowQuestion(text, caption: caption, buttons: MessageBoxButtons.YesNo);
            });
            return flag;
        }

        #endregion MessageBox

        #region 文件、目录

        [ThreadStatic]
        private static String lastSelectedFolder = String.Empty;

        public static Boolean SelectFile(this Form owner, String title, String filter, out String filePath,
                    String initialDirectory = "")
        {
            using (var dialog = new OpenFileDialog()
            {
                Title = title,
                InitialDirectory = initialDirectory,
                Multiselect = false,
                Filter = filter,
                CheckFileExists = true,
            })
            {
                Boolean isValid = false;
                SafeInvoke(owner, () => isValid = dialog.ShowDialog(owner) == DialogResult.OK);
                filePath = isValid ? dialog.FileName : null;
                return isValid;
            }
        }

        public static Boolean SelectFiles(this Form owner, String title, String filter, out String[] filePaths,
            String initialDirectory = "")
        {
            using (var dialog = new OpenFileDialog()
            {
                Title = title,
                InitialDirectory = initialDirectory,
                Multiselect = true,
                Filter = filter,
                CheckFileExists = true,
            })
            {
                Boolean isValid = false;
                SafeInvoke(owner, () => isValid = dialog.ShowDialog(owner) == DialogResult.OK);
                filePaths = isValid ? dialog.FileNames : null;
                return isValid;
            }
        }

        public static Boolean SelectFolder(this Form owner, String description, out String folder)
        {
            using (var dialog = new FolderBrowserDialog()
            {
                Description = description,
                ShowNewFolderButton = true,
                SelectedPath = lastSelectedFolder
            })
            {
                Boolean isValid = false;
                SafeInvoke(owner, () => isValid = dialog.ShowDialog(owner) == DialogResult.OK);
                if (isValid)
                {
                    folder = lastSelectedFolder = dialog.SelectedPath;
                }
                else
                {
                    folder = null;
                }
                return isValid;
            }
        }

        public static Boolean GetFileSavePath(this Form owner, String fileName, String filter,
            out String savePath)
        {
            Boolean isValid = !String.IsNullOrWhiteSpace(filter);
            if (isValid)
            {
                fileName = String.Concat(fileName, DateTime.Now.ToString("_yyyyMMdd_HHmmss"));
                using (var dialog = new SaveFileDialog()
                {
                    FileName = fileName,
                    Title = "保存文件",
                    Filter = filter,
                    OverwritePrompt = true,
                    CheckPathExists = true
                })
                {
                    SafeInvoke(owner, () => isValid = dialog.ShowDialog(owner) == DialogResult.OK);
                    savePath = isValid ? dialog.FileName : null;
                }
            }
            else
            {
                savePath = null;
            }
            return isValid;
        }

        public static void SaveFile(this Form owner, String fileName, IEnumerable<String> contents,
            String confirmInfo, String filter = "txt文件|*.txt")
        {
            Boolean isInvalid = contents.Count() > 0; if (isInvalid) { return; }
            isInvalid = String.IsNullOrWhiteSpace(confirmInfo)
                ? false
                : !Confirm(owner, confirmInfo, "保存确认？");
            if (isInvalid) { return; }
            if (GetFileSavePath(owner, fileName, filter, out String savePath))
            {
                try
                {
                    File.AppendAllLines(savePath, contents);
                    OpenPathAndSelectPath(savePath);
                }
                catch (Exception ex)
                {
                    AlertError(owner, ex, "文件保存失败");
                }
            }
        }

        public static Boolean SaveFile(this Form owner, String savePath, Action<FileStream> doWrite)
        {
            Boolean flag;
            FileStream file = null;
            try
            {
                file = new FileStream(savePath, FileMode.Create); doWrite(file); flag = true;
            }
            catch (Exception ex)
            {
                owner.AlertError(ex); flag = false;
            }
            finally
            {
                if (file != null) { file.Close(); file.Dispose(); }
            }
            return flag;
        }

        #endregion 文件、目录

        #region 其它

        public static void SafeInvoke(this Form owner, Action action)
        {
            if (!owner.IsHandleCreated || owner.IsDisposed) { return; }
            if (owner.InvokeRequired) { owner.Invoke(action); } else { action(); }
        }

        public static void SafeBeginInvoke(this Form owner, Action action)
        {
            if (!owner.IsHandleCreated || owner.IsDisposed) { return; }
            if (owner.InvokeRequired) { owner.BeginInvoke(action); } else { action(); }
        }

        public static void SetDoubleBuffered(this Control control)
        {
            try
            {
                control.GetType()
                       .GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                       .SetValue(control, true, null);
            }
            catch { }
        }

        /// <summary>打开并选中指定路径</summary>
        public static void OpenPathAndSelectPath(String path)
        {
            IntPtr pidlList = IntPtr.Zero;
            try
            {
                pidlList = WinApi.ILCreateFromPathW(path);
                if (pidlList != IntPtr.Zero)
                {
                    Int32 code = WinApi.SHOpenFolderAndSelectItems(pidlList, 0, IntPtr.Zero, 0);
                    Marshal.ThrowExceptionForHR(code);
                }
            }
            catch
            {
            }
            finally
            {
                WinApi.ILFree(pidlList);
            }
        }

        #endregion 其它

        #region Loading

        /// <summary>处理一些工作（显示加载中窗体）</summary>
        /// <param name="owner">所属窗体，为null时将不执行操作</param>
        /// <param name="message">消息</param>
        /// <param name="action">要执行的委托，为null时将不执行操作</param>
        public static void RunWithLoading(this Form owner, String message = "操作中", Action action = null)
        {
            if (owner == null || action == null)
            {
                return;
            }

            Task.Run(() =>
            {
                SafeInvoke(owner, () =>
                {
                    try
                    {
                        MaskHelper.Show(owner, message);
                        action?.Invoke();
                    }
                    catch (Exception ex)
                    {
                        AlertError(owner, ex.Message);
                    }
                    finally
                    {
                        MaskHelper.Hide(owner);
                    }
                });
            });
        }

        #endregion Loading
    }
}