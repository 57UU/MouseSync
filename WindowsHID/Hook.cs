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
/// <summary>
/// 设置的钩子类型
/// </summary>
public enum HookType : int
{
    /// <summary>
    /// WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以监视菜单，滚动
    ///条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。
    ///WH_MSGFILTER Hook只能监视传递到菜单，滚动条，消息框的消息，以及传递到通
    ///过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook
    ///监视所有应用程序消息。
    ///
    ///WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以在模式循环期间
    ///过滤消息，这等价于在主消息循环中过滤消息。
    ///
    ///通过调用CallMsgFilter function可以直接的调用WH_MSGFILTER Hook。通过使用这
    ///个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循
    ///环里一样
    /// </summary>
    WH_MSGFILTER = -1,
    /// <summary>
    /// WH_JOURNALRECORD Hook用来监视和记录输入事件。典型的，可以使用这
    ///个Hook记录连续的鼠标和键盘事件，然后通过使用WH_JOURNALPLAYBACK Hook
    ///来回放。WH_JOURNALRECORD Hook是全局Hook，它不能象线程特定Hook一样
    ///使用。WH_JOURNALRECORD是system-wide local hooks，它们不会被注射到任何行
    ///程地址空间
    /// </summary>
    WH_JOURNALRECORD = 0,
    /// <summary>
    /// WH_JOURNALPLAYBACK Hook使应用程序可以插入消息到系统消息队列。可
    ///以使用这个Hook回放通过使用WH_JOURNALRECORD Hook记录下来的连续的鼠
    ///标和键盘事件。只要WH_JOURNALPLAYBACK Hook已经安装，正常的鼠标和键盘
    ///事件就是无效的。WH_JOURNALPLAYBACK Hook是全局Hook，它不能象线程特定
    ///Hook一样使用。WH_JOURNALPLAYBACK Hook返回超时值，这个值告诉系统在处
    ///理来自回放Hook当前消息之前需要等待多长时间（毫秒）。这就使Hook可以控制实
    ///时事件的回放。WH_JOURNALPLAYBACK是system-wide local hooks，它们不会被
    ///注射到任何行程地址空间
    /// </summary>
    WH_JOURNALPLAYBACK = 1,
    /// <summary>
    /// 在应用程序中，WH_KEYBOARD Hook用来监视WM_KEYDOWN and
    ///WM_KEYUP消息，这些消息通过GetMessage or PeekMessage function返回。可以使
    ///用这个Hook来监视输入到消息队列中的键盘消息
    /// </summary>
    WH_KEYBOARD = 2,
    /// <summary>
    /// 应用程序使用WH_GETMESSAGE Hook来监视从GetMessage or PeekMessage函
    ///数返回的消息。你可以使用WH_GETMESSAGE Hook去监视鼠标和键盘输入，以及
    ///其它发送到消息队列中的消息
    /// </summary>
    WH_GETMESSAGE = 3,
    /// <summary>
    /// 监视发送到窗口过程的消息，系统在消息发送到接收窗口过程之前调用
    /// </summary>
    WH_CALLWNDPROC = 4,
    /// <summary>
    /// 在以下事件之前，系统都会调用WH_CBT Hook子过程，这些事件包括：
    ///1. 激活，建立，销毁，最小化，最大化，移动，改变尺寸等窗口事件；
    ///2. 完成系统指令；
    ///3. 来自系统消息队列中的移动鼠标，键盘事件；
    ///4. 设置输入焦点事件；
    ///5. 同步系统消息队列事件。
    ///Hook子过程的返回值确定系统是否允许或者防止这些操作中的一个
    /// </summary>
    WH_CBT = 5,
    /// <summary>
    /// WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以监视菜单，滚动
    ///条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。
    ///WH_MSGFILTER Hook只能监视传递到菜单，滚动条，消息框的消息，以及传递到通
    ///过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook
    ///监视所有应用程序消息。
    ///
    ///WH_MSGFILTER 和 WH_SYSMSGFILTER Hooks使我们可以在模式循环期间
    ///过滤消息，这等价于在主消息循环中过滤消息。
    ///
    ///通过调用CallMsgFilter function可以直接的调用WH_MSGFILTER Hook。通过使用这
    ///个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循
    ///环里一样
    /// </summary>
    WH_SYSMSGFILTER = 6,
    /// <summary>
    /// WH_MOUSE Hook监视从GetMessage 或者 PeekMessage 函数返回的鼠标消息。
    ///使用这个Hook监视输入到消息队列中的鼠标消息
    /// </summary>
    WH_MOUSE = 7,
    /// <summary>
    /// 当调用GetMessage 或 PeekMessage 来从消息队列种查询非鼠标、键盘消息时
    /// </summary>
    WH_HARDWARE = 8,
    /// <summary>
    /// 在系统调用系统中与其它Hook关联的Hook子过程之前，系统会调用
    ///WH_DEBUG Hook子过程。你可以使用这个Hook来决定是否允许系统调用与其它
    ///Hook关联的Hook子过程
    /// </summary>
    WH_DEBUG = 9,
    /// <summary>
    /// 外壳应用程序可以使用WH_SHELL Hook去接收重要的通知。当外壳应用程序是
    ///激活的并且当顶层窗口建立或者销毁时，系统调用WH_SHELL Hook子过程。
    ///WH_SHELL 共有５钟情况：
    ///1. 只要有个top-level、unowned 窗口被产生、起作用、或是被摧毁；
    ///2. 当Taskbar需要重画某个按钮；
    ///3. 当系统需要显示关于Taskbar的一个程序的最小化形式；
    ///4. 当目前的键盘布局状态改变；
    ///5. 当使用者按Ctrl+Esc去执行Task Manager（或相同级别的程序）。
    ///
    ///按照惯例，外壳应用程序都不接收WH_SHELL消息。所以，在应用程序能够接
    ///收WH_SHELL消息之前，应用程序必须调用SystemParametersInfo function注册它自
    ///己
    /// </summary>
    WH_SHELL = 10,
    /// <summary>
    /// 当应用程序的前台线程处于空闲状态时，可以使用WH_FOREGROUNDIDLE
    ///Hook执行低优先级的任务。当应用程序的前台线程大概要变成空闲状态时，系统就
    ///会调用WH_FOREGROUNDIDLE Hook子过程
    /// </summary>
    WH_FOREGROUNDIDLE = 11,
    /// <summary>
    /// 监视发送到窗口过程的消息，系统在消息发送到接收窗口过程之后调用
    /// </summary>
    WH_CALLWNDPROCRET = 12,
    /// <summary>
    /// 监视输入到线程消息队列中的键盘消息
    /// </summary>
    WH_KEYBOARD_LL = 13,
    /// <summary>
    /// 监视输入到线程消息队列中的鼠标消息
    /// </summary>
    WH_MOUSE_LL = 14
}

public delegate IntPtr LowLevelProc(int nCode, IntPtr wParam, IntPtr lParam);
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
    public int mouseData;//this is type DWORD,32 bit,use ushort to split it
    //public short highOrderMouseData;
    //public short LowOrderMouseData;
    public uint flags;
    public uint time;
    public IntPtr dwExtraInfo;
}
public record MouseInputData
{
    public MSLLHOOKSTRUCT hookStruct { set; get; }
    public MouseMessagesHook mouseMessage { get {
            return (MouseMessagesHook)code;
        } }
    
    public int code { set; get; }
}

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
[StructLayout(LayoutKind.Sequential)]
public struct KBDLLHOOKSTRUCT
{
    public int vkCode;
    public int scanCode;
    public int flags;
    public int time=0;
    public IntPtr dwExtraInfo;

    public KBDLLHOOKSTRUCT()
    {
    }
}
public record KeyboardInputData
{
    public KBDLLHOOKSTRUCT HookStruct;
    public KeyboardMessages KeyboardMessage { get {
            return (KeyboardMessages)code;
        } }
    public int code;
}
public enum KeyboardMessages:int
{
    WM_KEYDOWN = 0x0100,
    WM_KEYUP = 0x0101,
    WM_SYSKEYDOWN=0x0104,
    WM_SYSKEYUP=0x0105
}
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
