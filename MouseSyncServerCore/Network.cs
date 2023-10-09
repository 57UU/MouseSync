using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace MouseSyncServerCore;

public static class Network
{
    public static bool isSupportIPv6 { get; private set; }
    public static string localIP { get; private set; }
    public static string? localIPv6 { get; private set; }
    public static string boardcastIP { get; private set; }
    //public static string? boardcastIPv6 { get; private set; }
    static readonly Socket socket;
    //static readonly Socket? socketv6;
    static readonly IPEndPoint iep1;
    //static readonly IPEndPoint? iep1v6;
    static readonly byte[] buffer ;
    //static readonly byte[]? bufferv6;
    static Network()
    {

        boardcastIP=getIPCast();
        Info.instance.Server_IP = localIP;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        iep1 = new IPEndPoint(IPAddress.Parse(boardcastIP), Info.instance.Boardcast_Port);
        buffer = Encoding.Unicode.GetBytes($"IP{DataExchange.SPLIT}{localIP}{DataExchange.SPLIT}{Info.instance.Server_Port}");
    }
    private static string getIPCast()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipv4 = hostEntry.AddressList[1];
        IPAddress? ipv6=null;
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily==AddressFamily.InterNetwork)
            {
                
                ipv4 = ip;
                break;
            }else if(ip.AddressFamily == AddressFamily.InterNetworkV6)
            {
                if (!ip.IsIPv6LinkLocal)
                {
                    ipv6=ip;
                }
            }
        }
        isSupportIPv6=ipv6!=null;
        if(isSupportIPv6 )
        {
            localIPv6 = ipv6.ToString();
        }
        
        localIP = ipv4.ToString();
        string[] splited = ipv4.ToString().Split(new char[] { '.' });
        return string.Concat(new string[]
        {
                splited[0],
                ".",
                splited[1],
                ".",
                splited[2],
                ".255"
        });
    }
    public static bool boardcast()
    {
        try
        {
            socket.SendTo(buffer, iep1);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Boardcast unsuccessful"+e.ToString());
            return false;
        }

    }

}
