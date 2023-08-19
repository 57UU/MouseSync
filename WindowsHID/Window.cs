using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Runtime.InteropServices;

namespace WindowsHID;
public static class Window
{
    // Import the necessary functions from user32.dll
    [DllImport("user32.dll")]
    public static extern IntPtr CreateWindowEx(
        int dwExStyle,
        string lpClassName,
        string lpWindowName,
        int dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    public static extern bool UpdateWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern bool DestroyWindow(IntPtr hWnd);

    // Import the GetMessage, TranslateMessage, and DispatchMessage functions
    [DllImport("user32.dll")]
    public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

    [DllImport("user32.dll")]
    public static extern bool TranslateMessage(ref MSG lpMsg);

    [DllImport("user32.dll")]
    public static extern IntPtr DispatchMessage(ref MSG lpMsg);

    // Define constants for window styles
    const int WS_OVERLAPPED = 0x00000000;
    const int WS_SYSMENU = 0x00080000;
    const int WS_CAPTION = 0x00C00000;
    const int WS_MINIMIZEBOX = 0x00020000;
    const int WS_MAXIMIZEBOX = 0x00010000;

    const int SW_SHOWNORMAL = 1;

    // Define the MSG structure
    [StructLayout(LayoutKind.Sequential)]
    public struct MSG
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public POINT pt;
    }

    // Define the POINT structure
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;
    }
    static Thread thread;
    public static void Create()
    {

        thread = new(Loop);
        thread.Start();

    }
    public static void Loop()
    {
        IntPtr hWnd = CreateWindowEx(
            0,
            "Static",
            "这是一个你应该看不见的隐藏窗口",
            WS_OVERLAPPED | WS_SYSMENU | WS_CAPTION | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            100, 100, 400, 300,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero
        );
        Hook.StartAll();
        //ShowWindow(hWnd, SW_SHOWNORMAL);

        UpdateWindow(hWnd);

        MSG msg;
        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
        {
            
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
        }
    }
    public static void Destroy()
    {
        thread.Interrupt();
        DestroyWindow(hWnd);
    }
     static IntPtr hWnd;
    static void Main()
    {
        IntPtr hWnd = CreateWindowEx(
            0,
            "Static",
            "Sample Window",
            WS_OVERLAPPED | WS_SYSMENU | WS_CAPTION | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            100, 100, 400, 300,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero
        );

        if (hWnd != IntPtr.Zero)
        {
            ShowWindow(hWnd, SW_SHOWNORMAL);
            UpdateWindow(hWnd);

            MSG msg;
            while (GetMessage(out msg, IntPtr.Zero, 0, 0) )
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }

            DestroyWindow(hWnd);
        }
    }
}
