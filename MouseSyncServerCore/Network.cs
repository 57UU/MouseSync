using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace MouseSyncServerCore;

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
