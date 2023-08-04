using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync.Client;

[Obsolete]
internal static class Mouse_old
{
    [DllImport("user32.dll")]
    public static extern nint SetCursorPos(nint dwPtr, int x, int y);
    public static void MoveMouseTo(int x, int y)
    {
        SetCursorPos(nint.Zero, x, y);
    }
    [DllImport("user32.dll")]
    public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
    public enum ClickButton { left, mid, right };
    public static void ClickMouse(ClickButton button)
    {
        if (button == ClickButton.left)//left
        {
            mouse_event(0x0002, 0, 0, 0, 0);
        }
        else if (button == ClickButton.right)//right
        {
            mouse_event(0x0004, 0, 0, 0, 0);
        }
        else if (button == ClickButton.mid)//mid
        {
            mouse_event(0x0008, 0, 0, 0, 0);
        }
        else
        {
            throw new ArgumentException("Invalid button number");
        }
    }
    public static void RightClickMouse()
    {
        mouse_event(0x0002 | 0x0004, 0, 0, 0, 0);
    }

}
