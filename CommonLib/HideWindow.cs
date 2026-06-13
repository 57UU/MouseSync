using System;
using System.Runtime.InteropServices;

namespace CommonLib;

public static class HideWindow
{

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetConsoleWindow();

    private const int SW_MINIMIZE = 6; // 最小化窗口
    public static void Hide()
    {
        IntPtr handle = GetConsoleWindow();
        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_MINIMIZE);
        }
        else
        {
            Console.WriteLine("警告：无法隐藏窗口，最新版Windows终端可能会导致这个问题");
        }
    }

}
