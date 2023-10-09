
using CommonLib;
using WindowsHID;

namespace MouseSyncServerCore;
public class ServerCore
{

    public LogHandler LogHandler { get; set; } = Console.WriteLine;
    ConnectionServer connectionServer;
    int port = Info.instance.Server_Port;
    public static ServerCore instance;
    Thread boardcastThread;

    private ServerCore(bool isCreateFakeWindowAndHook = true, bool isHook = false) {
        hotkeyManager = new(2, switchPause);
        instance = this;
        if (isCreateFakeWindowAndHook)
        {
            Window.Create();
        }
        if (isHook)
        {
            WindowsHID.Hook.StartAll();
        }
        MouseHook.addCallback(mouseHandler);
        KeyboardHook.addCallback(keyHandler);

        ConnectionHandler handler = conn =>
        {
            var c = new ClientPC(conn, (s, b) =>
            {

                ClientPC c = (ClientPC)s;

                if ((!string.IsNullOrEmpty(c.Name)) && (!string.IsNullOrEmpty(c.Resolution)))
                {
                    printTable();
                    LogHandler($"{clients.Count}\t\t\t{c.Name}\t\t{c.Resolution}\t{c.IP}");
                }

            });

        };
        connectionServer = new(port, handler, Network.isSupportIPv6);
        LogHandler($"Listenning on port {port} ");

        connectionServer.OnError += e => Console.Error.WriteLine(e.ToString());

        MouseHook.maxCount = Info.instance.MouseMovingRate;
        ClientRemove += ServerCore_ClientRemove;
        LogHandler($"Local IPv4 is {Network.localIP}");
        if (Network.isSupportIPv6)
        {
            LogHandler($"Local IPv6 is {Network.localIPv6}");
        }
        else
        {
            LogHandler("Your device do NOT support IPv6");
        }
        if (Info.instance.IsEnableBoardcast)
        {
            LogHandler("Starting Boardcast");
            boardcastThread = new(() => {

                while (true)
                {
                    if (Network.boardcast())
                    {
                        if (Entry.isDebug)
                        {
                            LogHandler("Boardcasting successful");
                        }
                    }
                    Thread.Sleep(2000);
                }
            }) { IsBackground = true };
            boardcastThread.Start();
        }

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
    public event EventHandler<ClientPC> ClientAdd = (s, e) => { };
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


    bool isPause = false;
    private void switchPause()
    {
        
        isPause = !isPause;
        Console.WriteLine((isPause ? "--Paused--" : "--Continuing--")+"----------Press Shift+F8 to change state");
    }
    HotkeyManager hotkeyManager;
    private void keyHandler(object? sender, KeyboardInputData e)
    {
        if (Entry.isDebug)
        {
            Console.WriteLine(e.HookStruct.vkCode+" "+e.code);
        }


        if (!isPause)
        {
            foreach (ClientPC pc in clients)
            {
                pc.sendKeyboard(e);
            }
        }
        if (Info.instance.IsEnableHotKey)
        {
            if (e.HookStruct.vkCode == 161 || e.HookStruct.vkCode == 160)//shift
            {
                hotkeyManager.setState(0, e.code == 256);
            }
            if (e.HookStruct.vkCode == 119)//F8
            {
                hotkeyManager.setState(1, e.code == 256);
            }
        }

    }

    private void mouseHandler(object sender, MouseInputData e)
    {
        if (isPause) return;
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