using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace WindowsHID;

public class Hook
{
    public static void StartAll()
    {
        MouseHook.Start();
        KeyboardHook.Start();
    }
}


public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);



public static class SystemLevel_IO
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(HookType idHook, LowLevelProc lpfn, IntPtr hMod, uint dwThreadId = 0);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetConsoleWindow();
}

public static class MouseHook
{
    static MouseHook()
    {
        
    }


    public static void addCallback(EventHandler<MouseInputData> handler)
    {
        MouseAction += handler;
    }

    private static IntPtr hookID = IntPtr.Zero;

    public static event EventHandler<MouseInputData>? MouseAction;

    public static void Start()
    {
        
        if (hookID == IntPtr.Zero)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                nint handle;
                handle= SystemLevel_IO.GetModuleHandle(curModule.ModuleName);
                //handle=SystemLevel_IO.GetConsoleWindow();
                hookID = SystemLevel_IO.SetWindowsHookEx(HookType.WH_MOUSE_LL, LowLevelMouseProcCallback, handle);
            }
        }
    }

    public static void Stop()
    {
        if (hookID != IntPtr.Zero)
        {
            SystemLevel_IO.UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
    }
    static int count = 0;
    public static int maxCount = 5;//for moving event,only (1/maxCount) of messages will be sent
    private static IntPtr LowLevelMouseProcCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        void triggerEvent()
        {
            MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<MSLLHOOKSTRUCT>(lParam);
            MouseAction?.Invoke(null, new MouseInputData() { code = (int)wParam, hookStruct = hookStruct });
        }
        if (nCode >= 0)
        {
            if ((MouseMessagesHook)wParam == MouseMessagesHook.WM_MOUSEMOVE)
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
        return SystemLevel_IO.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
    }
}


//keyboard

public class KeyboardHook
{
    static KeyboardHook()
    {
        
    }
    public static event EventHandler<KeyboardInputData> KeyboardAction;

    private static IntPtr hookID = IntPtr.Zero;
    public static void addCallback(EventHandler<KeyboardInputData> handler)
    {
        KeyboardAction += handler;
    }   
    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 )
        {
            KBDLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            KeyboardAction?.Invoke(null,new KeyboardInputData() 
                {code=(int)wParam,HookStruct=hookStruct });
        }
        return SystemLevel_IO.CallNextHookEx(nint.Zero, nCode, wParam, lParam);
    }

    public static void Start()
    {
        if (hookID == IntPtr.Zero)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                hookID = SystemLevel_IO.SetWindowsHookEx(HookType.WH_KEYBOARD_LL, HookCallback, SystemLevel_IO.GetModuleHandle(curModule.ModuleName));
            }
        }
    }

    public static void Stop()
    {
        if (hookID != IntPtr.Zero)
        {
            SystemLevel_IO.UnhookWindowsHookEx(hookID);
            hookID = IntPtr.Zero;
        }
    }


}
