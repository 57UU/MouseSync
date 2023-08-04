using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MouseSync.Server;

public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

public enum MouseMessages
{
    WM_MOUSEMOVE = 0x0200,
    WM_LBUTTONDOWN = 0x0201,
    WM_LBUTTONUP = 0x0202,
    WM_RBUTTONDOWN = 0x0204,
    WM_RBUTTONUP = 0x0205,
    WM_MBUTTONDOWN = 0x0207,
    WM_MBUTTONUP = 0x0208,
}

[StructLayout(LayoutKind.Sequential)]
public struct POINT
{
    public int X;
    public int Y;
}

[StructLayout(LayoutKind.Sequential)]
public struct MSLLHOOKSTRUCT
{
    public POINT pt;
    public uint mouseData;
    public uint flags;
    public uint time;
    public IntPtr dwExtraInfo;
}
public static class IO {
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
}

public class MouseHook
{
    public static readonly MouseHook instance = new MouseHook();
    static MouseHook()
    {
        instance = new();
        instance.Start();
    }
    private MouseHook() { }
    
    public static void addCallback(EventHandler<MSLLHOOKSTRUCT> handler)
    {
        instance.MouseAction += handler;
    }
    private const int WH_MOUSE_LL = 14;
    private IntPtr hookID = IntPtr.Zero;

    public event EventHandler<MSLLHOOKSTRUCT>? MouseAction;

    public void Start()
    {
        if (hookID == IntPtr.Zero)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID = IO.SetWindowsHookEx(WH_MOUSE_LL, LowLevelMouseProcCallback, IO.GetModuleHandle(curModule.ModuleName), 0);
            }
        }
    }

    public void Stop()
    {
        if (hookID != IntPtr.Zero)
        {
            IO.UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
    }
    int count = 0;
    public int maxCount = 5;//for moving event,only (1/maxCount) of messages will be sent
    private IntPtr LowLevelMouseProcCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        void triggerEvent()
        {
            MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            MouseAction?.Invoke(this, hookStruct);
        }
        if (nCode >= 0)
        {
            if ((MouseMessages)wParam == MouseMessages.WM_MOUSEMOVE)
            {
                if (count == maxCount + 1 || count > maxCount + 1)
                {
                    count = 0;
                }
                if (count == 0)
                {
                    triggerEvent();
                }
                count++;
            }
            else
            {
                triggerEvent();
            }

        }

        return IO.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }
}

public class MouseEventArgs : EventArgs
{
    public nint eventNum { get; private set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public uint flags { get; private set; }

    public MouseEventArgs(nint eventNum,int x, int y, uint flags)
    {
        this.eventNum = eventNum;
        X = x;
        Y = y;
        this.flags = flags;
    }
}

