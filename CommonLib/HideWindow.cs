using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace CommonLib;

public static class HideWindow
{

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    private const int SW_MINIMIZE = 6; // 最小化窗口
    public static void Hide()
    {
        nint handle;
        handle = FindWindow(null, Console.Title);
        //handle=curProcess.MainWindowHandle;
        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_MINIMIZE);
        }
        else
        {
            throw new Exception("can not hide");
        }
    }

}
