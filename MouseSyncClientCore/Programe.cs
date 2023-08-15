using CommonLib;
using MouseSync;
using MouseSync.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MouseSyncClientCore;

public class Programe
{
    //entry point
    public static void Main()
    {
#if DEBUG
        ConsoleHelper.AllocConsole();
#endif
        if (string.IsNullOrEmpty(Info.instance.Server_IP))
        {
            Console.Write("Input Server IP: ");
            Info.instance.Server_IP = Console.ReadLine();
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
                Console.Error.WriteLine("\nPress any Key to Continue except Exit(e): ");

                var input = Console.ReadKey().KeyChar;
                if(input=='e')
                {
                    break;
                }
            }
            
        }
     

    }
}
