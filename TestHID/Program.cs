using WindowsHID;


//Console.WriteLine((uint)123);
while (true)
{
    var sb=Console.ReadLine();
    var sb2 = sb.Split(" ");
    int x = int.Parse(sb2[0]);
    int y = int.Parse(sb2[1]);
    MOUSEINPUT mouseInput = new();
    mouseInput.dy = y;
    mouseInput.dx = x;
    mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_MOVE;
    Input.sendMouseInput(mouseInput);
/*    InputForMouse.simulate(InputForMouse.Flags.MOUSEEVENTF_MOVE|InputForMouse.Flags.MOUSEEVENTF_ABSOLUTE,
        x, y);*/
}