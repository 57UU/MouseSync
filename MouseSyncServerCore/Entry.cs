using System.Linq;

namespace MouseSyncServerCore;

public enum ServerFlags
{
    debug
}
public class Entry
{
    static Entry()
    {
        
    }
    public static bool isDebug = false;
    //Entry
    public static void Main(string[] args)
    {
        Info.load();
        if (args.Contains(ServerFlags.debug.ToString()))
        {
            isDebug = true;
        }
        if (Info.instance.IsHideOnStart)
        {
            HideWindow.Hide();
        }
        ServerCore.start();
        


    }
}
