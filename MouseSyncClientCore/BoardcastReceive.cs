﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using CommonLib;

namespace MouseSyncClientCore;

public static class BoardcastReceive
{
    public static bool isReceived=false;
    public static Thread thread;
    public static void activate()
    {
        if(Info.instance.IsEnableBoardcast)
        {
            Console.WriteLine("Receiving Boardcast");
            thread=new Thread(Receive) { IsBackground=true};
            thread.Start(); 
        }
    }
    private static void Receive()
    {
        for (; ; )
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipendPoint = new IPEndPoint(IPAddress.Any, Info.instance.Boardcast_Port);
                EndPoint endPoint = ipendPoint;
                try
                {
                    socket.Bind(ipendPoint);
                }
                catch (Exception)
                {
                    Console.WriteLine("!程序已经运行，请勿多次运行！");
                    //Thread.CurrentThread.Interrupt();
                }
                byte[] array = new byte[1024];
                for (; ; )
                {
                    int num = socket.ReceiveFrom(array, ref endPoint);
                    if (num > 0)
                    {
                        string[] array2 = Encoding.Unicode.GetString(array, 0, num).Split(DataExchange.SPLIT);
                        if (array2[0] == "IP")
                        {
                            var tempIP = array2[1];
                            var tempPort =int.Parse( array2[2]);
                            if(tempIP!=Info.instance.Server_IP || tempPort!=Info.instance.Server_Port)
                            {
                                Info.instance.Server_IP = tempIP;
                                Info.instance.Server_Port = tempPort;
                                Console.WriteLine($"Server Address Update to {Info.instance.Server_IP_Port}");
                            } 
                            isReceived = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Thread.Sleep(1000);
        }
    }
}
