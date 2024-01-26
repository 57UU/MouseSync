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
    notsimulate,debug
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
        Info.load();

        Advertisement.Start();
        
        if (isContain(args,ClientFlags.notsimulate))
        {
            
            ClientNetwork.isSimulate = false;
        }
        if (isContain(args, ClientFlags.debug))
        {
            isDebug = true;
        }

        if(Info.instance.IsEnableBoardcast)
        {
            BoardcastReceive.activate();
        }
        if(!Info.instance.IsEnableBoardcast)
        {
            if (string.IsNullOrEmpty(Info.instance.Server_IP))
            {
                setServerIP();
            }
        }

        if(Info.instance.IsHideOnStart)
        {
            HideWindow.Hide();
        }
        if (Info.instance.IsEnableBoardcast)
        {
            Console.WriteLine("Waiting for boardcast");
            while (true)
            {
                if (BoardcastReceive.isReceived)
                {
                    break;
                }
                Thread.Sleep(50);
            }
        }


        
            

        Console.WriteLine("Try Connecting to "+Info.instance.Server_IP_Port);
        
        while (true)
        {
            try
            {
                new ClientNetwork(Info.instance.Server_IP, Info.instance.Server_Port);
            }catch (Exception e)
            {
                Console.WriteLine("Error: "+e.Message+"\n");
                


                if(!Info.instance.IsRetryInstantly)
                {
                    Console.Error.Write("\nPress any Key to Continue except Exit(e),Change Server IP(c): ");
                    var input = Console.ReadKey().KeyChar;
                    Console.WriteLine();
                    if (input == 'e')
                    {
                        break;
                    }
                    else if (input == 'c')
                    {
                        setServerIP();
                    }
                }
                else
                {
                    Console.WriteLine("--Retrying--");
                }
            }
            
        }
        Advertisement.Reset();
    }
}
