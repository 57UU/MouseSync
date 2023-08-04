using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync.Server;

internal class Network
{
    public static string localIP { get; private set; }
    public static string boardcastIP { get; private set; }
    static readonly Socket s;
    static readonly IPEndPoint iep1;
    static readonly byte[] buffer ;
    static Network()
    {
        if(Program.Status == Status.server)
        {
            boardcastIP=getIPCast();
            Info.instance.Server_IP = localIP;
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            iep1 = new IPEndPoint(IPAddress.Parse(getIPCast()), Info.instance.Boardcast_Port);
            buffer = Encoding.Unicode.GetBytes($"IP:{boardcastIP}");
        }
    }
    private static string getIPCast()
    {
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipv4 = hostEntry.AddressList[1];
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.ToString().Contains("."))
            {
                ipv4 = ip;
                break;
            }
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
            s.SendTo(buffer, iep1);
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

}
public class SocketServer
{
    
    int port;
    public SocketServer(int port)
    {
        this.port=port;
    }
    public async void Start()
    {
        try
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine("Server started. Waiting for a connection...");
            while (true)
            {
                try
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    ClientPC pc = new(client);
                }
                catch (Exception) { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}