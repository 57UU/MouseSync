using CommonLib;
using MouseSync;
using MouseSync.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;
using static CommonLib.Utils;

namespace MouseSyncClientCore;

public enum ClientFlags
{
    notSimulate,debug
}
public class Programe
{
    public static bool isDebug = false;
    private static void setServerIP()
    {
            Console.Write("Input Server IP: ");
            Info.instance.Server_IP = Console.ReadLine();
        Info.save();

    }
    //entry point
    public static void Main(string[] args)
    {
        if (args.Contains(ClientFlags.notSimulate.ToString()))
        {
            ClientNetwork.isSimulate = false;
        }
        if (isContain(args, ClientFlags.debug))
        {
            isDebug = true;
        }

#if DEBUG
        ConsoleHelper.AllocConsole();
#endif
        if (string.IsNullOrEmpty(Info.instance.Server_IP)) {
            setServerIP();
        }
            

        Console.WriteLine("Try Connecting to "+Info.instance.Server_IP_Port);
        
        while (true)
        {
            try
            {
                new ClientNetwork(Info.instance.Server_IP, Info.instance.Server_Port);
            }catch (Exception e)
            {
                Console.WriteLine("Error: "+e.ToString());
                Console.Error.WriteLine("\nPress any Key to Continue except Exit(e),Change Server IP(c): ");

                var input = Console.ReadKey().KeyChar;
                if(input=='e')
                {
                    break;
                }else if (input == 'c')
                {
                    setServerIP();
                }
            }
            
        }
     

    }
}
