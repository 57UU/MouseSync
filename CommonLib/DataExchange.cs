using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsHID;

namespace CommonLib;

public static class DataExchange
{
    static DataExchange()
    {

    }
    //for client
    public const string MOUSE= "m";
    public const string KEY = "k";
    public static readonly Dictionary<int, MOUSEEVENTF> MOUSE_KEY_MAP = new()
    {
        { (int)MouseMessagesHook.WM_MBUTTONUP,  MOUSEEVENTF.MOUSEEVENTF_MIDDLEUP },
        { (int)MouseMessagesHook.WM_MBUTTONDOWN, MOUSEEVENTF.MOUSEEVENTF_MIDDLEDOWN },
        { (int)MouseMessagesHook.WM_LBUTTONUP, MOUSEEVENTF.MOUSEEVENTF_LEFTUP },
        { (int)MouseMessagesHook.WM_LBUTTONDOWN, MOUSEEVENTF.MOUSEEVENTF_LEFTDOWN },
        { (int)MouseMessagesHook.WM_RBUTTONUP, MOUSEEVENTF.MOUSEEVENTF_RIGHTUP },
        { (int)MouseMessagesHook.WM_RBUTTONDOWN, MOUSEEVENTF.MOUSEEVENTF_RIGHTDOWN },
        { (int)MouseMessagesHook.WM_MOUSEMOVE, MOUSEEVENTF.MOUSEEVENTF_MOVE },
        { (int)MouseMessagesHook.WM_MOUSEWHEEL,MOUSEEVENTF.MOUSEEVENTF_WHEEL },
    };
    public static readonly Dictionary<int, KEYEVENTF> KEYEVENT_MAP = new()
    {
        {(int)KeyboardMessages.WM_KEYUP,KEYEVENTF.KEYEVENTF_KEYUP },
        {(int)KeyboardMessages.WM_KEYDOWN,KEYEVENTF.KEYEVENTF_KEYDOWN },
        {(int)KeyboardMessages.WM_SYSKEYDOWN,KEYEVENTF.KEYEVENTF_KEYDOWN },
        {(int)KeyboardMessages.WM_SYSKEYUP,KEYEVENTF.KEYEVENTF_KEYUP },
    };
    //for both
    public static readonly string SPLIT=":";
    public static readonly string EOF = ((char)4).ToString();
    //for server
    public const string NAME="name";
    public const string RESOLUTION = "re";
    //public const string DESCRIPTION = "description";
}
