namespace CommonLib;

public class HotkeyManager
{
    bool[] hotkeys;
    ThreadStart action;
    public HotkeyManager(int numKeys,ThreadStart task)
    {
        hotkeys = new bool[numKeys];
        for (int i = 0; i < numKeys; i++)
        {
            hotkeys[i] = false;
        }
        action = task;
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

            if(index==0)
            {
                if (isAllKeyDown)
                {
                    action.Invoke();
                }
                isAllKeyDown = false;
            }
        }
    }

}
