using System.Runtime.InteropServices;

namespace WindowsHID;

public enum HookType : int
{
    WH_MSGFILTER = -1,
    WH_JOURNALRECORD = 0,
    WH_JOURNALPLAYBACK = 1,
    WH_KEYBOARD = 2,
    WH_GETMESSAGE = 3,
    WH_CALLWNDPROC = 4,
    WH_CBT = 5,
    WH_SYSMSGFILTER = 6,
    WH_MOUSE = 7,
    WH_HARDWARE = 8,
    WH_DEBUG = 9,
    WH_SHELL = 10,
    WH_FOREGROUNDIDLE = 11,
    WH_CALLWNDPROCRET = 12,
    WH_KEYBOARD_LL = 13,
    WH_MOUSE_LL = 14
}

[Flags]
public enum MouseMessagesHook : int
{
    WM_MOUSEMOVE = 0x0200,
    WM_LBUTTONDOWN = 0x0201,
    WM_LBUTTONUP = 0x0202,
    WM_RBUTTONDOWN = 0x0204,
    WM_RBUTTONUP = 0x0205,
    WM_MBUTTONDOWN = 0x0207,
    WM_MBUTTONUP = 0x0208,
    WM_MOUSEWHEEL = 0x020A,
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
    public int mouseData;
    public uint flags;
    public uint time;
    public IntPtr dwExtraInfo;
}

public record MouseInputData
{
    public MSLLHOOKSTRUCT hookStruct { set; get; }
    public MouseMessagesHook mouseMessage
    {
        get { return (MouseMessagesHook)code; }
    }
    public int code { set; get; }
}

[StructLayout(LayoutKind.Sequential)]
public struct KBDLLHOOKSTRUCT
{
    public int vkCode;
    public int scanCode;
    public int flags;
    public int time = 0;
    public IntPtr dwExtraInfo;

    public KBDLLHOOKSTRUCT()
    {
    }
}

public record KeyboardInputData
{
    public KBDLLHOOKSTRUCT HookStruct;
    public KeyboardMessages KeyboardMessage
    {
        get { return (KeyboardMessages)code; }
    }
    public int code;
}

public enum KeyboardMessages : int
{
    WM_KEYDOWN = 0x0100,
    WM_KEYUP = 0x0101,
    WM_SYSKEYDOWN = 0x0104,
    WM_SYSKEYUP = 0x0105
}
