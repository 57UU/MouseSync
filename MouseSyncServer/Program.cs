using MouseSyncServerCore;
using WindowsHID;

public class Programe
{
    public static void Main(string[] args)
    {
        Entry.Main(args);
        var instance = ServerCore.instance;
        bool isCreateFakeWindowAndHook=true, isHook=false;

        if ((isCreateFakeWindowAndHook) || (isHook))
        {
            MouseHook.addCallback(instance.mouseHandler);
            KeyboardHook.addCallback(instance.keyHandler);
        }
        if (isCreateFakeWindowAndHook)
        {
            Window.Create();
        }
        if (isHook)
        {
            Hook.StartAll();
        }
        ServerCore.wait();

    }
}