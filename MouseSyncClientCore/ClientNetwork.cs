
using CommonLib;
using MouseSyncClientCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        if (Programe.isDebug)
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
    public static  bool isSimulate=true;
    private void handleMouseEvent(string[] msg)
    {
        int button = int.Parse(msg[1]);
        int x = int.Parse(msg[2]);
        int y = int.Parse(msg[3]);
        var mouseData = int.Parse(msg[4]);
        MOUSEINPUT mouseInput = new();
        mouseInput.dy = y;
        mouseInput.dx = x;
        mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_MOVE;

        if (button==(int)MouseMessagesHook.WM_MOUSEWHEEL) {
            if (Programe.isDebug)
            {
                LogHandler("Simulate wheel");
            }

            //InputForMouse.simulate(InputForMouse.Flags.MOUSEEVENTF_WHEEL, x, y,mouseData);
            mouseInput.dwFlags = MOUSEEVENTF.MOUSEEVENTF_WHEEL;
            mouseInput.mouseData = mouseData>>16;

           
        }
        else
        {
            if (Programe.isDebug)
            {
                LogHandler("simulate btn press");
            }


            //InputForMouse.simulate(DataExchange.MOUSE_KEY_MAP[button], x, y);
            if (DataExchange.MOUSE_KEY_MAP.ContainsKey(button))
            {
                mouseInput.dwFlags = DataExchange.MOUSE_KEY_MAP[button];
            }
            else
            {
                LogHandler("Error:can not prase :" + button);
            }
        }
        if (isSimulate)
        {
            Input.sendMouseInput(mouseInput);
        }
        

    }
/*    [Obsolete]
    private void handleMouseEvent_obsolete(string[] msg)
    {
        int button = int.Parse(msg[1]);

        int x= int.Parse(msg[2]);
        int y= int.Parse(msg[3]);
        var mouseData= int.Parse(msg[4]);
        MOUSEINPUT mouse_input=new MOUSEINPUT();
        mouse_input.dy = y;
        mouse_input.dx = x; 
        mouse_input.mouseData = mouseData;
        Input.sendMouseInput(mouse_input);
    }*/
    private void handleKeyboardEvent(string[] msg)
    {
        int code = int.Parse(msg[1]);
        var vkcode = ushort.Parse(msg[2]);
        var scanCode = ushort.Parse(msg[3]);
        if(DataExchange.KEYEVENT_MAP.ContainsKey(code))
        {
            if(Programe.isDebug)
            {
                LogHandler($"Key receive: {vkcode}");
            }

            var flag = DataExchange.KEYEVENT_MAP[code];
            var input = new Input.KEYBDINPUT() {
                wVk = vkcode,
                wScan = scanCode,
                dwFlags = (uint)flag
            };
            if(isSimulate)
            Input.sendKeyboardInput(input);
        }
        else
        {
            LogHandler("Unknown Key");
        }
        
    }
}
