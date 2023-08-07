using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync.Client;

internal static class UserInput
{

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
    private static void sendInputSealed(INPUT[] inputs)
    {
        SendInput((uint)inputs.Length, inputs,INPUT.Size);
    }
    private static void sendOneInput(INPUT input)
    {
        sendInputSealed(new INPUT[] {input});
    }
    public static void sendMouseInput(MOUSEINPUT mouseInput)
    {
        INPUT input=new INPUT();
        input.type = InputType.INPUT_MOUSE;
        mouseInput.dwFlags = mouseInput.dwFlags | MOUSEEVENTF.MOUSEEVENTF_ABSOLUTE;
        input.U = new() {mi=mouseInput };
        sendOneInput(input);
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

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public MOUSEEVENTF dwFlags;
        public uint time=0;
        public IntPtr dwExtraInfo;

        public MOUSEINPUT()
        {
        }
    }

    [Flags]
    public enum MOUSEEVENTF : uint
    {
        LEFTDOWN = 0x0002,
        LEFTUP = 0x0004,
        RIGHTDOWN = 0x0008,
        RIGHTUP = 0x0010,
        MOUSEEVENTF_ABSOLUTE=0x8000
    }
}
