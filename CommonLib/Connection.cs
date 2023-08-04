using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MouseSync;

public class Connection
{
    public delegate void MessageHander(string msg);
    
    public readonly TcpClient client;
    private NetworkStream stream;
    public MessageHander? messageHander { get; set; }
    public Connection(TcpClient client)
    {
        this.client = client;
        stream = client.GetStream();
        receiveTask = receive();
    }

    public Task receiveTask {  get; private set; }
    public async void send(string msg)
    {
        byte[] responseData = Encoding.UTF8.GetBytes(msg+DataExchange.delimiter);
        await stream.WriteAsync(responseData, 0, responseData.Length);
    }
    private async Task receive()
    {
        while (true)
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                //Console.WriteLine($"Received: {dataReceived}");
                var m = dataReceived.Split(DataExchange.delimiter);
                for(int i=0;i<m.Length-1; i++)
                {
                    messageHander?.Invoke(m[i]);
                }

            }
        }
    }
}
