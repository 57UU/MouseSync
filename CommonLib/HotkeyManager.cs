using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib;

public class HotkeyManager
{
    bool[] hotkeys;
    ThreadStart Task;
    public HotkeyManager(int numKeys,ThreadStart task)
    {
        hotkeys = new bool[numKeys];
        for (int i = 0; i < numKeys; i++)
        {
            hotkeys[i] = false;
        }
        Task = task;
    }
    bool isAllKeyDown=false;
    /// <summary>
    /// when one of hot key states is changed,invoke this 
    /// </summary>
    /// <param name="index">index of the key,starts with 0</param>
    /// <param name="States">True is Keydown,False is Keyup</param>
    public void setState(int index,bool States)
    {
        hotkeys[index] = States;
        if(States)
        {//key down
            bool flag = true;
            for(int i = 0;i<hotkeys.Length;i++)
            {
                flag &= hotkeys[i];
            }
            isAllKeyDown = flag;
        }
        else
        {//keyup
            bool flag = false;
            for(int i = 0;i < hotkeys.Length;i++)
            {
                flag |= hotkeys[i];
            }
            if(!flag)
            {
                if (isAllKeyDown)
                {
                    Task.Invoke();
                }
                isAllKeyDown = false;
            }
        }
    }

}
