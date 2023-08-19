using CommonLib;
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
Window.Create();
MouseHook.addCallback((s, b) =>
{
    Console.WriteLine(b.code);
});
KeyboardHook.addCallback((s, b) =>
{
    Console.WriteLine(b.code);
});
new Thread(() => { Thread.Sleep(int.MaxValue); }).Start();