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

        var instance = ServerCore.CreateInstance();
        try
        {
            instance.Launch();
            if (Info.instance.IsHideOnStart)
            {
                HideWindow.Hide();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("发生错误: ");
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }



    }
}
