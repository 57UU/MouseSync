
using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace MouseSync.Client;

public  delegate void LogHandler(string message);
public class ClientNetwork
{
    
    public static LogHandler LogHandler { set; get; } = Console.WriteLine;
    
    public Connection Connection { get; private set; }
    public ClientNetwork(in string ip,in int port) {
        //isSimulate = false;
        Connection = Connection.connect(ip,port,receive);
        Connection.onError += Connection_onError;
        //send the resolution and machine name
        sendPairData(DataExchange.RESOLUTION, Device.Resolution);
        sendPairData(DataExchange.NAME, Device.MachineName);
        /*        while(!Connection.receiveTask.IsCompleted)
                {
                    Thread.Sleep(100);

                }*/
        int result=Connection.receiveTask.Result;
        if(result == 0)
        {
            throw new Exception("Cancelled by user");
        }
        
    }

    private void Connection_onError(Exception e)
    {
        LogHandler("Connection Failed");
        throw e;
    }

    private void sendPairData(string prefix,string content)
    {
        Connection.send($"{prefix}{DataExchange.SPLIT}{content}");
    }
    private void receive(string msg)
    {
#if DEBUG
        LogHandler("Received: "+msg);
#endif
        var splited=msg.Split(DataExchange.SPLIT);
        if (splited[0] == DataExchange.MOUSE)
        {
            handleMouseEvent(splited);
        }else if (splited[0] == DataExchange.KEY_BOARD) {
            handleKeyboardEvent(splited);
        }
    
    }
    public bool isSimulate=true;
    private void handleMouseEvent(string[] msg)
    {
        int button = int.Parse(msg[1]);
        int x = int.Parse(msg[2]);
        int y = int.Parse(msg[3]);
        var mouseData = uint.Parse(msg[4]);

        if(button==(int)MouseMessagesHook.WM_MOUSEWHEEL) {
#if DEBUG
            LogHandler("Simulate wheel");
#endif
            if(isSimulate) {  
                //InputForMouse.simulate(InputForMouse.Flags.MOUSEEVENTF_WHEEL, x, y,mouseData);
            }
           
        }
        else
        {
#if DEBUG
            LogHandler("simulate btn press");
#endif
            if (isSimulate)
            {
                //InputForMouse.simulate(DataExchange.MOUSE_KEY_MAP[button], x, y);
            }

            
        }
        

    }
    [Obsolete]
    private void handleMouseEvent_obsolete(string[] msg)
    {
        int button = int.Parse(msg[1]);

        int x= int.Parse(msg[2]);
        int y= int.Parse(msg[3]);
        int mouseData= int.Parse(msg[4]);
        MOUSEINPUT mouse_input=new MOUSEINPUT();
        mouse_input.dy = y;
        mouse_input.dx = x; 
        mouse_input.mouseData = mouseData;
        Input.sendMouseInput(mouse_input);
    }
    private void handleKeyboardEvent(string[] msg)
    {

    }
}
