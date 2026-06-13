
using CommonLib;
using MouseSyncClientCore;
using WindowsHID;

namespace MouseSync.Client;


public class ClientNetwork
{
    
    public static LogHandler LogHandler { set; get; } = Console.WriteLine;
    
    public Connection Connection { get; private set; }
    public event EventHandler onLoaded=(s,b)=>LogHandler("Connected to server successfully");
    public ClientNetwork(in string ip,in int port) {
        //isSimulate = false;
        Connection = Connection.connect(ip,port,receive);
        Connection.onError += Connection_onError;
        //send the resolution and machine name
        Connection.send("ok");
        sendPairData(DataExchange.RESOLUTION, Device.Resolution);
        sendPairData(DataExchange.NAME, Device.MachineName);
        Connection.StartReceive();
        onLoaded?.Invoke(this,EventArgs.Empty);
        int result=Connection.receiveTask.Result;
        if(result == 0)
        {
            throw new Exception("Cancelled by user");
        }

        
    }

    private void Connection_onError(Exception e)
    {
        LogHandler("Connection has been interrupted,disconnected form server\n");
        throw e;
    }

    private void sendPairData(string prefix,string content)
    {
        Connection.send($"{prefix}{DataExchange.SPLIT}{content}");
    }
    private void receive(string msg)
    {
        if (Program.isDebug)
        {
            LogHandler("Received: " + msg);
        }
        var splited=msg.Split(DataExchange.SPLIT);
        try{
            if (splited[0] == DataExchange.MOUSE)
            {

                handleMouseEvent(splited);
            }
            else if (splited[0] == DataExchange.KEY)
            {
                handleKeyboardEvent(splited);
            }
        }catch(Exception e)
        {
            LogHandler($"Error Parse: {e.Message}");
        }
    
    }
    public static bool isSimulate = true;
    private void handleMouseEvent(string[] msg)
    {
        if (msg.Length < 5 || !int.TryParse(msg[1], out int button)
            || !int.TryParse(msg[2], out int x) || !int.TryParse(msg[3], out int y)
            || !int.TryParse(msg[4], out int mouseData))
        {
            LogHandler("Error: invalid mouse event data");
            return;
        }

        MOUSEINPUT mouseInput = new();
        mouseInput.dy = y;
        mouseInput.dx = x;
        mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_MOVE;

        if (button == (int)MouseMessagesHook.WM_MOUSEWHEEL)
        {
            if (Program.isDebug)
            {
                LogHandler("Simulate wheel");
            }
            mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_WHEEL;
            mouseInput.mouseData = mouseData >> 16;
        }
        else
        {
            if (Program.isDebug)
            {
                LogHandler("simulate btn press");
            }
            if (DataExchange.MOUSE_KEY_MAP.ContainsKey(button))
            {
                mouseInput.dwFlags = DataExchange.MOUSE_KEY_MAP[button];
            }
            else
            {
                LogHandler("Error: cannot parse button " + button);
            }
        }
        if (isSimulate)
        {
            Input.sendMouseInput(mouseInput);
        }
    }

    private void handleKeyboardEvent(string[] msg)
    {
        if (msg.Length < 4 || !int.TryParse(msg[1], out int code)
            || !ushort.TryParse(msg[2], out ushort vkcode)
            || !ushort.TryParse(msg[3], out ushort scanCode))
        {
            LogHandler("Error: invalid keyboard event data");
            return;
        }

        if (DataExchange.KEYEVENT_MAP.ContainsKey(code))
        {
            if (Program.isDebug)
            {
                LogHandler($"Key receive: {vkcode}");
            }
            var flag = DataExchange.KEYEVENT_MAP[code];
            var input = new Input.KEYBDINPUT()
            {
                wVk = vkcode,
                wScan = scanCode,
                dwFlags = (uint)flag
            };
            if (isSimulate)
                Input.sendKeyboardInput(input);
        }
        else
        {
            LogHandler("Unknown Key");
        }
    }
}
