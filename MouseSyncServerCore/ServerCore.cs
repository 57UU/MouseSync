
using CommonLib;
using WindowsHID;

namespace MouseSyncServerCore;
public class ServerCore
{

    public  LogHandler LogHandler { get; set; } = Console.WriteLine;
    ConnectionServer connectionServer;
    int port = Info.instance.Server_Port;
    public static ServerCore instance;

    private ServerCore() {
        instance = this;
        Window.Create();
        MouseHook.addCallback(mouseHandler);
        KeyboardHook.addCallback(keyHandler);
        
        connectionServer =
            new(port, conn => { 
                var c=new ClientPC(conn, (s, b) => {
                    ClientPC c = (ClientPC)s;
                    if ((!string.IsNullOrEmpty(c.Name)) && (!string.IsNullOrEmpty(c.Resolution)))
                    {
                        printTable();
                        LogHandler($"{clients.Count}\t\t\t{c.Name}\t\t{c.Resolution}\t{c.IP}");
                    }
                });

            });
        
        connectionServer.OnError += e => Console.Error.WriteLine(e.ToString());

        MouseHook.maxCount=Info.instance.MouseMovingRate;
        ClientRemove += ServerCore_ClientRemove;

        LogHandler("----------Server is Ready----------");
        //printTable();
        
    }
    private void printTable()
    {
        LogHandler("\nAll Connected Devices\tMachine Name\tResolution\tIP Address");
    }
    public static void Start_Wait()
    {
        new ServerCore();
        ServerCore.instance.connectionServer.thread.Join();
    }

    private void ServerCore_ClientRemove(object? sender, ClientPC e)
    {
        LogHandler($"Device Offline: {e.Name}");
        LogHandler($"{clients.Count} devices still connected");
    }

    List<ClientPC> clients = new List<ClientPC>();
    public readonly object globalLock = new();
    public event EventHandler<ClientPC> ClientAdd = (s,b)=>{  };
    public event EventHandler<ClientPC> ClientRemove;
    public void addClient(ClientPC pc)
    {
        
        lock (globalLock)
        {
            clients.Add(pc);
        }
        ClientAdd.Invoke(this, pc);
    }
    public void removeClient(ClientPC pc)
    {
        
        lock (globalLock)
        {
            clients.Remove(pc);
        }
        ClientRemove.Invoke(this, pc);
    }
    private void keyHandler(object? sender, KeyboardInputData e)
    {
        foreach (ClientPC pc in clients)
        {
            pc.sendKeyboard(e);
        }
    }

    private void mouseHandler(object sender, MouseInputData e)
    {

        foreach (ClientPC pc in clients)
        {
            pc.sendMouse(e);
        }
    }
    public void Stop()
    {
        connectionServer.close();
        Window.Destroy();
    }
}  