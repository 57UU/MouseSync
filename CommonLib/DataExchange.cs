using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace MouseSync;

public static class DataExchange
{
    static DataExchange()
    {

    }
    //for client
    public const string MOUSE= "m";
    public const string KEY_BOARD = "k";
    public static readonly Dictionary<int, InputForMouse.Flags> MOUSE_KEY_MAP = new()
    {
        { (int)MouseMessagesHook.WM_MBUTTONUP,  InputForMouse.Flags.MOUSEEVENTF_MIDDLEUP },
        { (int)MouseMessagesHook.WM_MBUTTONDOWN, InputForMouse.Flags.MOUSEEVENTF_MIDDLEDOWN },
        { (int)MouseMessagesHook.WM_LBUTTONUP, InputForMouse.Flags.MOUSEEVENTF_LEFTUP },
        { (int)MouseMessagesHook.WM_LBUTTONDOWN, InputForMouse.Flags.MOUSEEVENTF_LEFTDOWN },
        { (int)MouseMessagesHook.WM_RBUTTONUP, InputForMouse.Flags.MOUSEEVENTF_RIGHTUP },
        { (int)MouseMessagesHook.WM_RBUTTONDOWN, InputForMouse.Flags.MOUSEEVENTF_RIGHTDOWN },
        { (int)MouseMessagesHook.WM_MOUSEMOVE, InputForMouse.Flags.MOUSEEVENTF_MOVE },
        { (int)MouseMessagesHook.WM_MOUSEWHEEL,InputForMouse.Flags.MOUSEEVENTF_WHEEL },
    };
    //for both
    public static readonly string SPLIT=":";
    public static readonly string EOF = ((char)4).ToString();
    //for server
    public const string NAME="name";
    public const string RESOLUTION = "re";
    //public const string DESCRIPTION = "description";


}
