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
    notsimulate,debug,ip
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
        
        if (isContainFlag(args,ClientFlags.notsimulate))
        {
            
            ClientNetwork.isSimulate = false;
        }
        if (isContainFlag(args, ClientFlags.debug))
        {
            isDebug = true;
        }
        bool isSetIpMannully = false;
        try
        {
            int index = findFlag(args, ClientFlags.ip);
            if (index != -1)
            {
                Info.instance.Server_IP=args[index+1];
                isSetIpMannully=true;
                Console.WriteLine($"CommandLine ip:{Info.instance.Server_IP}");
            }
        }
        catch (Exception ex) { 
            Console.Error.WriteLine("unknown parameter for ip");
        }
        if (Info.instance.IsHideOnStart)
        {
            HideWindow.Hide();
        }
        if (!isSetIpMannully) {
            if (Info.instance.IsEnableBroadcast)
            {
                BroadcastReceive.activate();
            }
            if (!Info.instance.IsEnableBroadcast)
            {
                if (string.IsNullOrEmpty(Info.instance.Server_IP))
                {
                    setServerIP();
                }
            }
            if (Info.instance.IsEnableBroadcast)
            {
                Console.WriteLine("Waiting for broadcast");
                while (true)
                {
                    if (BroadcastReceive.isReceived)
                    {
                        break;
                    }
                    Thread.Sleep(50);
                }
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
    }
}
