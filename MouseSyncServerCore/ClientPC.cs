using System.Net;
using System.Net.Sockets;

namespace MouseSyncServerCore;

public class ClientPC
{
    public Connection Connection { get; private set; }
    public event EventHandler<string> onMessgeReceived;
    public ClientPC(Connection connection, EventHandler<string> onMessageReceived)
    {
        connection.messageHandler = received;
        this.onMessgeReceived = onMessageReceived;
        this.tcp = connection.TCPclient;
        this.IP = ((IPEndPoint)(tcp.Client.RemoteEndPoint)).Address.ToString();

        ServerCore.instance.addClient(this);

        Connection = connection;

        connection.onError += Connection_onError;
        connection.StartReceive();
    }

    private void Connection_onError(Exception e)
    {
        //Console.WriteLine("Lost Connection"+e.ToString());
        ServerCore.instance.removeClient(this);
    }


    //this is a call back
    public void received(string msg)
    {
        if (Entry.isDebug)
        {
            Console.WriteLine("Received: " + msg);
        }

        var splited = msg.Split(DataExchange.SPLIT);
        if (splited.Length > 1)
        {
            if (splited[0] == DataExchange.RESOLUTION)
            {
                Resolution = splited[1];
            }
            else if (splited[0] == DataExchange.NAME)
            {
                Name = splited[1];
            }
        }
        onMessgeReceived.Invoke(this, msg);

    }
    //send data format: (x:y:mousedata:flags)
    public void sendMouse(MouseInputData e)
    {
        //Connection.send($"{e.pt.X}:{e.pt.Y}:{e.flags}:{e}");
        Connection.send(Utils.format(
            DataExchange.MOUSE,
            e.code,
            e.hookStruct.pt.X,
            e.hookStruct.pt.Y,
            e.hookStruct.mouseData
            ));
    }
    public void sendKeyboard(KeyboardInputData data)
    {
        Connection.send(Utils.format(
            DataExchange.KEY,
            data.code,//key down or up
            data.HookStruct.vkCode,
            data.HookStruct.scanCode
            ));
    }

    public string Name { get; set; } = string.Empty;
    public string Resolution { get; set; } = string.Empty;
    public string IP { get; set; } = string.Empty;
    public TcpClient tcp { get; set; }

}
