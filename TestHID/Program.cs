using CommonLib;
using System.Diagnostics;
using WindowsHID;

/*while (true)
{
    var sb = Console.ReadLine();
    var sb2 = sb.Split(" ");
    int x = int.Parse(sb2[0]);
    int y = int.Parse(sb2[1]);

    MOUSEINPUT mouseInput = new();
    mouseInput.dy = y;
    mouseInput.dx = x;
    mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_MOVE;
    Input.sendMouseInput(mouseInput);
*//*    InputForMouse.simulate(InputForMouse.Flags.MOUSEEVENTF_MOVE|InputForMouse.Flags.MOUSEEVENTF_ABSOLUTE,
        x, y);*//*
}*/
/*string programPath = Process.GetCurrentProcess().MainModule.FileName;
Console.WriteLine("Program Path: " + programPath);
Window.Create();
MouseHook.addCallback((s, b) =>
{
    Console.WriteLine(b.code);
});
KeyboardHook.addCallback((s, b) =>
{
    Console.WriteLine(b.code);
});
new Thread(() => { Thread.Sleep(int.MaxValue); }).Start();*/

//Configuration.Config.Test();
HotkeyManager hotkeyManager = new(2, () => { Console.WriteLine("invoked"); });
hotkeyManager.setState(1, true);
Console.WriteLine("1");
hotkeyManager.setState(0, true);
Console.WriteLine("1");
hotkeyManager.setState(1, false);
Console.WriteLine("1");
hotkeyManager.setState(0, false);
