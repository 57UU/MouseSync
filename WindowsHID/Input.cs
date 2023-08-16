using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
namespace WindowsHID;



public static class InputForMouse
{
    [DllImport("user32")]
    private static extern int mouse_event(uint dwFlags, int dx, int dy, uint mouseData, IntPtr dwExtraInfo);
    private static int mouse_event(Flags flags, int x, int y,uint data=0)
    {
        return mouse_event((uint)(flags |Flags.MOUSEEVENTF_ABSOLUTE), x, y,data,0);
    }
    [Flags]
    public enum Flags : uint
    {
        MOUSEEVENTF_MOVE = 0x0001, //移动鼠标
        MOUSEEVENTF_LEFTDOWN = 0x0002, //模拟鼠标左键按下
        MOUSEEVENTF_LEFTUP = 0x0004, //模拟鼠标左键抬起
        MOUSEEVENTF_RIGHTDOWN = 0x0008, //模拟鼠标右键按下
        MOUSEEVENTF_RIGHTUP = 0x0010, //模拟鼠标右键抬起
        MOUSEEVENTF_MIDDLEDOWN = 0x0020, //模拟鼠标中键按下
        MOUSEEVENTF_MIDDLEUP = 0x0040,//模拟鼠标中键抬起
        MOUSEEVENTF_ABSOLUTE = 0x8000,// 标示是否采用绝对坐标
        MOUSEEVENTF_WHEEL=0x0800,
    }
    public static void simulate(Flags flags, int x, int y,uint data=0)
    {
        mouse_event(flags, x, y,data);
    }
}

public static class Input
{
/*    [Flags]
    public enum MouseMessageInput : int
    {
        MOUSEEVENTF_MOVE = 0x0001,

        MOUSEEVENTF_LEFTDOWN = 0x0002,

        MOUSEEVENTF_LEFTUP = 0x0004,

        MOUSEEVENTF_RIGHTDOWN = 0x0008,

        MOUSEEVENTF_RIGHTUP = 0x0010,

        MOUSEEVENTF_MIDDLEDOWN = 0x0020,

        MOUSEEVENTF_MIDDLEUP = 0x0040,

        MOUSEEVENTF_XDOWN = 0x0080,

        MOUSEEVENTF_XUP = 0x0100,

        MOUSEEVENTF_WHEEL = 0x0800,

        MOUSEEVENTF_HWHEEL = 0x1000,

        MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,

        MOUSEEVENTF_VIRTUALDESK = 0x4000,

        MOUSEEVENTF_ABSOLUTE = 0x8000
    }*/

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    private static void sendInputSealed(INPUT[] inputs)
    {
        
        SendInput((uint)inputs.Length,inputs,INPUT.Size);
    }
    private static void sendOneInput(INPUT input)
    {
        sendInputSealed(new INPUT[] {input});
    }
    public static void sendMouseInput(MOUSEINPUT mouseInput)
    {
        INPUT input=new INPUT();
        input.type = InputType.INPUT_MOUSE;
        convertLocation(ref mouseInput);
        mouseInput.dwFlags = mouseInput.dwFlags | MOUSEEVENTF.MOUSEEVENTF_ABSOLUTE;
        input.U = new() {mi=mouseInput };
        sendOneInput(input);
    }
    public const int max = 65535;
    public static void convertLocation(ref MOUSEINPUT mouseInput)
    {
        int width = Device.width;
        int height = Device.height;
        mouseInput.dx = (int)(mouseInput.dx / (float)width * max);
        mouseInput.dy = (int)(mouseInput.dy/(float)height*max);
    }
    public static void sendKeyboardInput(KEYBDINPUT keyboardInput)
    {
        INPUT input = new INPUT();
        input.type = InputType.INPUT_KETBOARD;
        input.U = new() { ki=keyboardInput };
        sendOneInput(input);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public int type;
        public InputUnion U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }
    public static class InputType 
    {
        public const int INPUT_MOUSE= 0;
        public const int INPUT_KETBOARD= 1;

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
    }
    // 定义KEYBDINPUT结构体，用于描述键盘输入信息
    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk; // 虚拟键码
        public ushort wScan; // 扫描码
        public uint dwFlags; // 标志位
        public uint time=0;
        public IntPtr dwExtraInfo;

        public KEYBDINPUT()
        {
        }
    }




}
[Flags]
public enum MOUSEEVENTF : uint
{
    MOUSEEVENTF_MOVE = 0x0001, //移动鼠标
    MOUSEEVENTF_LEFTDOWN = 0x0002, //模拟鼠标左键按下
    MOUSEEVENTF_LEFTUP = 0x0004, //模拟鼠标左键抬起
    MOUSEEVENTF_RIGHTDOWN = 0x0008, //模拟鼠标右键按下
    MOUSEEVENTF_RIGHTUP = 0x0010, //模拟鼠标右键抬起
    MOUSEEVENTF_MIDDLEDOWN = 0x0020, //模拟鼠标中键按下
    MOUSEEVENTF_MIDDLEUP = 0x0040,//模拟鼠标中键抬起
    MOUSEEVENTF_ABSOLUTE = 0x8000,// 标示是否采用绝对坐标
    MOUSEEVENTF_WHEEL = 0x0800,
}
[StructLayout(LayoutKind.Sequential)]
public struct MOUSEINPUT
{
    public int dx;
    public int dy;
    public int mouseData;
    public MOUSEEVENTF dwFlags;
    public uint time = 0;
    public IntPtr dwExtraInfo;

    public MOUSEINPUT()
    {
    }
}