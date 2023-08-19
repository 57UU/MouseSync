using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        if (args.Contains(ServerFlags.debug.ToString()))
        {
            isDebug = true;
        }
        ServerCore.Start_Wait();
    }
}
