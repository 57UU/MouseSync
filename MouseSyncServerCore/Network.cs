using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace MouseSyncServerCore;

[Obsolete]
public static class _Network
{
    public static bool isSupportIPv6 { get; private set; }
    public static string localIP { get; private set; }
    public static string? localIPv6 { get; private set; }
    public static string broadcastIP { get; set; }
    //public static string? boardcastIPv6 { get; private set; }
    static readonly Socket socket;
    //static readonly Socket? socketv6;
    static readonly IPEndPoint iep1;
    //static readonly IPEndPoint? iep1v6;
    static readonly byte[] buffer ;
    //static readonly byte[]? bufferv6;
    static _Network()
    {

        broadcastIP=getIPCast();
        Info.instance.Server_IP = localIP;

        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        iep1 = new IPEndPoint(IPAddress.Parse(broadcastIP), Info.instance.Broadcast_Port);
        buffer = Encoding.Unicode.GetBytes($"IP{DataExchange.SPLIT}{localIP}{DataExchange.SPLIT}{Info.instance.Server_Port}");
    }
    private static string getIPCast()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipv4;
        if (hostEntry.AddressList.Length>1)
        {
            ipv4 = hostEntry.AddressList[1];
        }
        else
        {
            ipv4 = hostEntry.AddressList[0];
        }
        
        IPAddress? ipv6=null;
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily==AddressFamily.InterNetwork)
            {
                
                ipv4 = ip;
                //break;
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
    public static bool broadcast()
    {
        try
        {
            socket.SendTo(buffer, iep1);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Broadcast unsuccessful"+e.ToString());
            return false;
        }

    }

}
public static class Networks
{
    static List<BroadcastUdp> broadcasts = new List<BroadcastUdp>();
    static List<IPAddress> ipv6s = new List<IPAddress>();
    public static bool IsSupportIPv6
    {
        get {
            return ipv6s.Count > 0;
        }
    } 
    static Networks()
    {
        try
        {
            GetLocalIP();
        }
        catch (Exception e) {
            Console.Error.WriteLine($"broadcast service error: {e.Message}");
        }
        
    }
    public static List<string> Ipv4s
    {
        get {
            return broadcasts
                .Select(i => i.localAddress.ToString())
                .ToList();
        }
    }
    public static List<string> Ipv6s
    {
        get
        {
            return ipv6s
                .Select(i => i.ToString())
                .ToList();
        }
    }
    // 计算广播地址
    static IPAddress GetBroadcastAddress(IPAddress address, IPAddress mask)
    {
        byte[] ipBytes = address.GetAddressBytes();
        byte[] maskBytes = mask.GetAddressBytes();
        byte[] broadcastBytes = new byte[ipBytes.Length];

        for (int i = 0; i < ipBytes.Length; i++)
        {
            broadcastBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
        }

        return new IPAddress(broadcastBytes);
    }
    static void GetLocalIP()
    {
        // 获取本机的所有网络接口和IP地址
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(ni => ni.OperationalStatus == OperationalStatus.Up)
            .ToList();

        // 遍历所有网络接口，创建UDP套接字并广播本机地址
        foreach (var networkInterface in networkInterfaces)
        {
            
            foreach (var ipInterface in networkInterface.GetIPProperties().UnicastAddresses)
            {
                if (ipInterface.Address.IsIPv6LinkLocal) {
                    continue;
                }
                if (ipInterface.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var ip = ipInterface.Address;
                    if (ip.GetAddressBytes()[0]==127) {//loopback 
                        continue ;
                    }
                    // 获取子网广播地址
                    var broadcastAddress = GetBroadcastAddress(ipInterface.Address,ipInterface.IPv4Mask);
                    
                    
                    BroadcastUdp broadcast = new(ip,broadcastAddress,Info.instance.Broadcast_Port);
                    broadcasts.Add(broadcast);
                }else if(ipInterface.Address.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    if (isIpv6Loopback(ipInterface.Address.GetAddressBytes()))
                    {
                        continue ;
                    }
                    ipv6s.Add(ipInterface.Address);
                }
            }
        }
        
    }
    private static bool isIpv6Loopback(byte[] ip)
    {
        if (ip.Length != 16) return false;
        byte[] loopback = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1];
        for (int i = 0; i < 16; i++) {
            if (ip[i] != loopback[i]) return false;
        }
        return true; 
    }
    public static bool broadcast()
    {
        try
        {
            foreach (var broadcast in broadcasts)
            {
                broadcast.Broadcast();
            }
            return true;
        }
        catch (Exception ex) {
            Console.WriteLine("Broadcast unsuccessful" + ex.ToString());
            return false;
        }

    }

}
class BroadcastUdp
{
    UdpClient udp;
    byte[] message;
    IPAddress broadcastAddress;
    public readonly IPAddress localAddress;
    int port;
    public readonly AddressFamily addressFamily;
    public BroadcastUdp(IPAddress localIp,IPAddress broadcastIp,int port)
    {
        localAddress=localIp;
        string msg = $"IP{DataExchange.SPLIT}{localIp.ToString()}{DataExchange.SPLIT}{Info.instance.Server_Port}";
        udp = new UdpClient();
        addressFamily=localIp.AddressFamily;
        udp.EnableBroadcast = true;
        this.message = Encoding.Unicode.GetBytes(msg);
        this.broadcastAddress = broadcastIp;
        this.port = port;
    }
    public void Broadcast()
    {
        udp.Send(message, message.Length, new IPEndPoint(broadcastAddress, port));
    }

}
