using WindowsHID;

public class Programe
{
    public static void Main(string[] args)
    {
        MouseSyncServerCore.Entry.Main(args);
        var instance = MouseSyncServerCore.ServerCore.instance;
        bool isCreateFakeWindowAndHook=true, isHook=false;

        if (isCreateFakeWindowAndHook)
        {
            Window.Create();
        }
        if (isHook)
        {
            Hook.StartAll();
        }
        if ((isCreateFakeWindowAndHook) || (isHook))
        {
            MouseHook.addCallback(instance.mouseHandler);
            KeyboardHook.addCallback(instance.keyHandler);
        }
    }
}