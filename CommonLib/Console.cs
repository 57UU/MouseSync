using System.Runtime.InteropServices;

namespace CommonLib;

public delegate void LogHandler(string message);
public static class ConsoleHelper
{
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();//关联一个控制台窗口
    [DllImport("kernel32.dll")]
    public static extern bool FreeConsole();//释放关联的控制台窗口
}
