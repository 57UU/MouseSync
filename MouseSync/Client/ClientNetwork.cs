using MouseSync.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync.Client;

public class ClientNetwork
{
    public Connection Connection { get; private set; }
    public ClientNetwork(string ip,int port) {
        TcpClient client = new TcpClient();
        client.Connect(IPAddress.Parse(ip), port);
        Connection=new Connection(client);
        Connection.messageHander += receive;
    }
    public void receive(string msg)
    {

    }
}
